using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using ModularPipelines.Engine;
using ModularPipelines.Logging;

namespace ModularPipelines.Console;

/// <summary>
/// Intercepts Console.Out/Error writes and routes them through the coordinator.
/// </summary>
/// <remarks>
/// <para>
/// <b>Purpose:</b> This writer replaces Console.Out/Error to catch all direct
/// console writes. During progress phase, writes are buffered per-module.
/// After progress ends, writes pass through directly.
/// </para>
/// <para>
/// <b>Module Detection:</b> Uses <see cref="ModuleLogger.CurrentModuleType"/> (AsyncLocal)
/// to detect which module (if any) is currently executing. This allows Console.WriteLine
/// calls inside modules to be attributed to the correct module's output buffer.
/// </para>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. All operations are either
/// read-only or delegated to thread-safe components.
/// </para>
/// </remarks>
[ExcludeFromCodeCoverage]
internal class CoordinatedTextWriter : TextWriter
{
    private static readonly AsyncLocal<bool> DirectWriteScope = new();

    private readonly IConsoleCoordinator _coordinator;
    private readonly TextWriter _realConsole;
    private readonly Func<bool> _shouldBuffer;
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly ISecretProvider _secretProvider;
    private readonly Dictionary<LineBufferKey, LineBufferState> _lineBuffers = [];
    private readonly object _lineBufferLock = new();
    private readonly object _outputLock = new();
    private SecretPatterns _secretPatterns = new([], null);
    private long? _secretPatternsVersion;

    /// <summary>
    /// Initialises a new instance of the <see cref="CoordinatedTextWriter"/> class.
    /// Initializes a new coordinated text writer.
    /// </summary>
    /// <param name="coordinator">The console coordinator.</param>
    /// <param name="realConsole">The real console to write to when not buffering.</param>
    /// <param name="shouldBuffer">Function that returns whether output should be buffered.</param>
    /// <param name="secretObfuscator">Obfuscator for secrets in output.</param>
    /// <param name="secretProvider">Provider for registered secret patterns.</param>
    public CoordinatedTextWriter(
        IConsoleCoordinator coordinator,
        TextWriter realConsole,
        Func<bool> shouldBuffer,
        ISecretObfuscator secretObfuscator,
        ISecretProvider secretProvider)
    {
        _coordinator = coordinator;
        _realConsole = realConsole;
        _shouldBuffer = shouldBuffer;
        _secretObfuscator = secretObfuscator;
        _secretProvider = secretProvider;
    }

    /// <inheritdoc />
    public override Encoding Encoding => _realConsole.Encoding;

    /// <inheritdoc />
    public override void WriteLine(string? value)
    {
        var state = GetLineBufferState();
        lock (state.SyncRoot)
        {
            WriteCore(state, (value ?? string.Empty).AsSpan(), appendNewLine: true);
        }
    }

    /// <inheritdoc />
    public override void WriteLine()
    {
        WriteLine(string.Empty);
    }

    /// <inheritdoc />
    public override void Write(string? value)
    {
        if (value == null)
        {
            return;
        }

        var state = GetLineBufferState();
        lock (state.SyncRoot)
        {
            WriteCore(state, value.AsSpan(), appendNewLine: false);
        }
    }

    /// <inheritdoc />
    public override void Write(char value)
    {
        var state = GetLineBufferState();
        lock (state.SyncRoot)
        {
            var shouldBuffer = GetBufferMode(state, ShouldBuffer());
            state.Buffer.Append(value);
            ProcessPendingOutput(state, shouldBuffer, shouldProcess: value == '\n');
        }
    }

    /// <inheritdoc />
    public override void Write(char[] buffer, int index, int count)
    {
        ArgumentNullException.ThrowIfNull(buffer);

        var state = GetLineBufferState();
        lock (state.SyncRoot)
        {
            WriteCore(state, buffer.AsSpan(index, count), appendNewLine: false);
        }
    }

    /// <inheritdoc />
    public override void Write(ReadOnlySpan<char> buffer)
    {
        var state = GetLineBufferState();
        lock (state.SyncRoot)
        {
            WriteCore(state, buffer, appendNewLine: false);
        }
    }

    /// <summary>
    /// Routes a message to the appropriate buffer based on current module context.
    /// </summary>
    private void RouteToBuffer(string message, Type? moduleType)
    {
        if (moduleType != null)
        {
            // Inside a module - route to that module's buffer
            var buffer = _coordinator.GetModuleBuffer(moduleType);
            buffer.WriteLine(message);
        }
        else
        {
            // Outside any module - route to unattributed buffer
            _coordinator.GetUnattributedBuffer().WriteLine(message);
        }
    }

    private void WriteCore(LineBufferState state, ReadOnlySpan<char> value, bool appendNewLine)
    {
        var shouldBuffer = GetBufferMode(state, ShouldBuffer());
        state.Buffer.Append(value);

        if (appendNewLine)
        {
            state.Buffer.Append(Environment.NewLine);
        }

        ProcessPendingOutput(
            state,
            shouldBuffer,
            shouldProcess: appendNewLine || value.Contains('\n'));
    }

    private void ProcessPendingOutput(LineBufferState state, bool shouldBuffer, bool shouldProcess)
    {
        if (shouldBuffer && !shouldProcess)
        {
            return;
        }

        var patterns = GetSecretPatterns();
        var retainedPrefixLength = GetPotentialPatternPrefixLength(state.Buffer, patterns.Values);
        retainedPrefixLength = ObfuscateCompletePatterns(state, patterns, retainedPrefixLength);
        FlushSafeOutput(state, retainedPrefixLength, shouldBuffer);
    }

    private LineBufferState GetLineBufferState()
    {
        var moduleType = ModuleLogger.CurrentModuleType.Value;
        var key = new LineBufferKey(moduleType, DirectWriteScope.Value);

        lock (_lineBufferLock)
        {
            if (!_lineBuffers.TryGetValue(key, out var state))
            {
                state = new LineBufferState(moduleType);
                _lineBuffers.Add(key, state);
            }

            return state;
        }
    }

    private static bool GetBufferMode(LineBufferState state, bool requestedBufferMode)
    {
        if (state.ShouldBuffer is null || state.Buffer.Length == 0)
        {
            state.ShouldBuffer = requestedBufferMode;
        }

        return state.ShouldBuffer.Value;
    }

    private SecretPatterns GetSecretPatterns()
    {
        lock (_lineBufferLock)
        {
            var version = _secretProvider.Version;
            if (_secretPatternsVersion is not null
                && (version & 1) == 0
                && _secretPatternsVersion == version
                && _secretProvider.Version == version)
            {
                return _secretPatterns;
            }

            var snapshot = _secretProvider.GetSnapshot();
            var values = (snapshot.Secrets ?? [])
                .Where(pattern => !string.IsNullOrEmpty(pattern))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderByDescending(pattern => pattern.Length)
                .ToArray();
            _secretPatterns = new SecretPatterns(
                values,
                values.Length == 0
                    ? null
                    : SearchValues.Create(values, StringComparison.OrdinalIgnoreCase));
            _secretPatternsVersion = snapshot.Version;
            return _secretPatterns;
        }
    }

    private int ObfuscateCompletePatterns(
        LineBufferState state,
        SecretPatterns patterns,
        int retainedPrefixLength,
        bool preservePotentialLongerMatch = true)
    {
        if (state.Buffer.Length == 0 || patterns.SearchValues is null)
        {
            return retainedPrefixLength;
        }

        if (_secretObfuscator is not ITrackedSecretObfuscator trackedObfuscator)
        {
            return retainedPrefixLength;
        }

        var pending = state.Buffer.ToString();
        if (pending.AsSpan().IndexOfAny(patterns.SearchValues) < 0)
        {
            return retainedPrefixLength;
        }

        var output = new StringBuilder(pending.Length);
        var outputIndex = 0;
        var searchIndex = 0;
        var replaced = false;
        var retainedPrefixInvalidated = false;
        var retainedPrefixStart = pending.Length - retainedPrefixLength;

        while (searchIndex < pending.Length)
        {
            var match = FindFirstPattern(pending, patterns, searchIndex);
            if (match.Index < 0)
            {
                break;
            }

            if (preservePotentialLongerMatch
                && retainedPrefixLength > 0
                && match.Index + match.Length > retainedPrefixStart)
            {
                var safeMatchLength = FindLongestPatternEndingAtOrBefore(
                    pending,
                    patterns.Values,
                    match.Index,
                    retainedPrefixStart);
                if (safeMatchLength == 0)
                {
                    searchIndex = match.Index + 1;
                    continue;
                }

                match = (match.Index, safeMatchLength);
            }

            var secret = pending.Substring(match.Index, match.Length);
            var obfuscation = trackedObfuscator.ObfuscateWithConsumption(secret, null);
            if (obfuscation.ConsumedInputLength == 0)
            {
                searchIndex = match.Index + 1;
                continue;
            }

            var unconsumedLength = match.Length - obfuscation.ConsumedInputLength;
            var obfuscatedLength = obfuscation.Output.Length - unconsumedLength;
            output.Append(pending, outputIndex, match.Index - outputIndex);
            output.Append(obfuscation.Output, 0, obfuscatedLength);
            retainedPrefixInvalidated |= match.Index + obfuscation.ConsumedInputLength > retainedPrefixStart;
            outputIndex = match.Index + obfuscation.ConsumedInputLength;
            searchIndex = outputIndex;
            replaced = true;
        }

        if (replaced)
        {
            output.Append(pending, outputIndex, pending.Length - outputIndex);
            state.Buffer.Clear();
            state.Buffer.Append(output);
        }

        return retainedPrefixInvalidated
            ? GetPotentialPatternPrefixLength(state.Buffer, patterns.Values)
            : retainedPrefixLength;
    }

    private void FlushSafeOutput(LineBufferState state, int retainedLength, bool shouldBuffer)
    {
        FlushSafePrefix(state, state.Buffer.Length - retainedLength, shouldBuffer);

        if (state.Buffer.Length == 0)
        {
            state.ShouldBuffer = null;
        }
    }

    private void FlushSafePrefix(LineBufferState state, int safeLength, bool shouldBuffer)
    {
        var consumedLength = 0;
        for (var index = 0; index < safeLength; index++)
        {
            if (state.Buffer[index] != '\n')
            {
                continue;
            }

            var lineLength = index - consumedLength;
            while (lineLength > 0 && state.Buffer[consumedLength + lineLength - 1] == '\r')
            {
                lineLength--;
            }

            var line = state.Buffer.ToString(consumedLength, lineLength);
            WriteCompletedLine(line, shouldBuffer, state.ModuleType);
            consumedLength = index + 1;
        }

        if (consumedLength > 0)
        {
            state.Buffer.Remove(0, consumedLength);
            safeLength -= consumedLength;
        }

        if (!shouldBuffer)
        {
            FlushDirectPrefix(state, safeLength);
        }
    }

    private static (int Index, int Length) FindFirstPattern(
        string input,
        SecretPatterns patterns,
        int startIndex)
    {
        var relativeIndex = input.AsSpan(startIndex).IndexOfAny(patterns.SearchValues!);
        if (relativeIndex < 0)
        {
            return (-1, 0);
        }

        var firstIndex = startIndex + relativeIndex;
        var matchingInput = input.AsSpan(firstIndex);
        foreach (var pattern in patterns.Values)
        {
            if (matchingInput.StartsWith(pattern, StringComparison.OrdinalIgnoreCase))
            {
                return (firstIndex, pattern.Length);
            }
        }

        throw new InvalidOperationException("SearchValues returned a position without a matching secret.");
    }

    private static int FindLongestPatternEndingAtOrBefore(
        string input,
        IReadOnlyList<string> patterns,
        int startIndex,
        int endIndex)
    {
        if (startIndex >= endIndex)
        {
            return 0;
        }

        var safeInput = input.AsSpan(startIndex, endIndex - startIndex);
        foreach (var pattern in patterns)
        {
            if (safeInput.StartsWith(pattern, StringComparison.OrdinalIgnoreCase))
            {
                return pattern.Length;
            }
        }

        return 0;
    }

    private static int GetPotentialPatternPrefixLength(StringBuilder input, IReadOnlyList<string> patterns)
    {
        if (input.Length == 0 || patterns.Count == 0)
        {
            return 0;
        }

        var maximumLength = patterns[0].Length - 1;
        var suffixLength = Math.Min(input.Length, maximumLength);

        if (suffixLength <= 256)
        {
            Span<char> suffixBuffer = stackalloc char[suffixLength];
            input.CopyTo(input.Length - suffixLength, suffixBuffer, suffixLength);
            return FindPotentialPatternPrefixLength(suffixBuffer, patterns);
        }

        var rentedBuffer = ArrayPool<char>.Shared.Rent(suffixLength);
        try
        {
            var suffixBuffer = rentedBuffer.AsSpan(0, suffixLength);
            input.CopyTo(input.Length - suffixLength, suffixBuffer, suffixLength);
            return FindPotentialPatternPrefixLength(suffixBuffer, patterns);
        }
        finally
        {
            ArrayPool<char>.Shared.Return(rentedBuffer, clearArray: true);
        }
    }

    private static int FindPotentialPatternPrefixLength(
        ReadOnlySpan<char> suffixBuffer,
        IReadOnlyList<string> patterns)
    {
        for (var length = suffixBuffer.Length; length > 0; length--)
        {
            var suffix = suffixBuffer[^length..];
            foreach (var pattern in patterns)
            {
                if (pattern.Length > length
                    && pattern.AsSpan().StartsWith(suffix, StringComparison.OrdinalIgnoreCase))
                {
                    return length;
                }
            }
        }

        return 0;
    }

    private void FlushDirectPrefix(LineBufferState state, int length)
    {
        if (length <= 0)
        {
            return;
        }

        var output = ObfuscateCustomOutput(state.Buffer.ToString(0, length));
        state.Buffer.Remove(0, length);
        lock (_outputLock)
        {
            _realConsole.Write(output);
        }
    }

    private void WriteCompletedLine(string line, bool shouldBuffer, Type? moduleType)
    {
        line = ObfuscateCustomOutput(line);

        if (shouldBuffer)
        {
            RouteToBuffer(line, moduleType);
        }
        else
        {
            lock (_outputLock)
            {
                _realConsole.WriteLine(line);
            }
        }
    }

    private void FlushPartialLine(LineBufferState state, bool shouldBuffer)
    {
        FlushPartialPrefix(state, state.Buffer.Length, shouldBuffer);
    }

    private void FlushPartialPrefix(LineBufferState state, int length, bool shouldBuffer)
    {
        if (length <= 0)
        {
            return;
        }

        var pending = ObfuscateCustomOutput(state.Buffer.ToString(0, length));
        state.Buffer.Remove(0, length);

        if (shouldBuffer)
        {
            RouteToBuffer(pending, state.ModuleType);
        }
        else
        {
            lock (_outputLock)
            {
                _realConsole.Write(pending);
            }
        }
    }

    private string ObfuscateCustomOutput(string output) =>
        _secretObfuscator is ITrackedSecretObfuscator
            ? output
            : _secretObfuscator.Obfuscate(output, null);

    /// <inheritdoc />
    public override void Flush()
    {
        LineBufferState[] states;
        lock (_lineBufferLock)
        {
            states = _lineBuffers.Values.ToArray();
        }

        var patterns = GetSecretPatterns();
        foreach (var state in states)
        {
            lock (state.SyncRoot)
            {
                if (state.Buffer.Length == 0)
                {
                    continue;
                }

                var shouldBuffer = state.ShouldBuffer ?? ShouldBuffer();
                var retainedPrefixLength = GetPotentialPatternPrefixLength(state.Buffer, patterns.Values);
                ObfuscateCompletePatterns(
                    state,
                    patterns,
                    retainedPrefixLength,
                    preservePotentialLongerMatch: false);
                FlushSafePrefix(state, state.Buffer.Length, shouldBuffer);
                FlushPartialLine(state, shouldBuffer);
                state.ShouldBuffer = null;
            }
        }

        // Always flush real console (needed for Spectre.Console internals)
        lock (_outputLock)
        {
            _realConsole.Flush();
        }
    }

    /// <summary>
    /// Flushes output that cannot be a prefix of a registered secret while retaining
    /// incomplete secret prefixes for subsequent writes.
    /// </summary>
    internal Task FlushAvailableAsync()
    {
        LineBufferState[] states;
        lock (_lineBufferLock)
        {
            states = _lineBuffers.Values.ToArray();
        }

        var patterns = GetSecretPatterns();
        foreach (var state in states)
        {
            lock (state.SyncRoot)
            {
                if (state.Buffer.Length == 0)
                {
                    continue;
                }

                var shouldBuffer = state.ShouldBuffer ?? ShouldBuffer();
                var retainedLength = GetPotentialPatternPrefixLength(state.Buffer, patterns.Values);
                retainedLength = ObfuscateCompletePatterns(
                    state,
                    patterns,
                    retainedLength,
                    preservePotentialLongerMatch: false);
                FlushSafeOutput(state, retainedLength, shouldBuffer);

                if (shouldBuffer)
                {
                    FlushPartialPrefix(state, state.Buffer.Length - retainedLength, shouldBuffer);
                }

                if (state.Buffer.Length == 0)
                {
                    state.ShouldBuffer = null;
                }
            }
        }

        lock (_outputLock)
        {
            _realConsole.Flush();
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task FlushAsync()
    {
        Flush();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Flush();
        }

        base.Dispose(disposing);
    }

    internal static IDisposable BeginDirectWrite()
    {
        var previousValue = DirectWriteScope.Value;
        DirectWriteScope.Value = true;
        return new DirectWriteScopeRestorer(previousValue);
    }

    private bool ShouldBuffer() => !DirectWriteScope.Value && _shouldBuffer();

    private sealed class LineBufferState(Type? moduleType)
    {
        public object SyncRoot { get; } = new();

        public Type? ModuleType { get; } = moduleType;

        public StringBuilder Buffer { get; } = new();

        public bool? ShouldBuffer { get; set; }
    }

    private readonly record struct LineBufferKey(Type? ModuleType, bool IsDirectWrite);

    private readonly record struct SecretPatterns(string[] Values, SearchValues<string>? SearchValues);

    private sealed class DirectWriteScopeRestorer(bool previousValue) : IDisposable
    {
        public void Dispose()
        {
            DirectWriteScope.Value = previousValue;
        }
    }
}
