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
    private readonly bool _isError;
    private readonly Dictionary<LineBufferKey, LineBufferState> _lineBuffers = [];
    private readonly object _lineBufferLock = new();
    private string[] _secretPatterns = [];
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
    /// <param name="isError">Whether this writer represents standard error.</param>
    public CoordinatedTextWriter(
        IConsoleCoordinator coordinator,
        TextWriter realConsole,
        Func<bool> shouldBuffer,
        ISecretObfuscator secretObfuscator,
        ISecretProvider secretProvider,
        bool isError = false)
    {
        _coordinator = coordinator;
        _realConsole = realConsole;
        _shouldBuffer = shouldBuffer;
        _secretObfuscator = secretObfuscator;
        _secretProvider = secretProvider;
        _isError = isError;
    }

    /// <inheritdoc />
    public override Encoding Encoding => _realConsole.Encoding;

    /// <inheritdoc />
    public override void WriteLine(string? value)
    {
        lock (_lineBufferLock)
        {
            WriteCore((value ?? string.Empty).AsSpan(), appendNewLine: true);
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

        lock (_lineBufferLock)
        {
            WriteCore(value.AsSpan(), appendNewLine: false);
        }
    }

    /// <inheritdoc />
    public override void Write(char value)
    {
        lock (_lineBufferLock)
        {
            var state = GetLineBufferState();
            var shouldBuffer = GetBufferMode(state, ShouldBuffer());
            state.Buffer.Append(value);
            ProcessPendingOutput(state, shouldBuffer, shouldProcess: value == '\n');
        }
    }

    /// <inheritdoc />
    public override void Write(char[] buffer, int index, int count)
    {
        ArgumentNullException.ThrowIfNull(buffer);

        lock (_lineBufferLock)
        {
            WriteCore(buffer.AsSpan(index, count), appendNewLine: false);
        }
    }

    /// <inheritdoc />
    public override void Write(ReadOnlySpan<char> buffer)
    {
        lock (_lineBufferLock)
        {
            WriteCore(buffer, appendNewLine: false);
        }
    }

    /// <summary>
    /// Routes a message to the appropriate buffer based on current module context.
    /// </summary>
    private void RouteToBuffer(string message, Type? moduleType, bool appendNewLine)
    {
        var buffer = moduleType is not null
            ? _coordinator.GetModuleBuffer(moduleType)
            : _coordinator.GetUnattributedBuffer();
        if (_isError)
        {
            if (appendNewLine)
            {
                buffer.WriteErrorLine(message);
            }
            else
            {
                buffer.WriteError(message);
            }
        }
        else if (appendNewLine)
        {
            buffer.WriteLine(message);
        }
        else
        {
            buffer.Write(message);
        }
    }

    private void WriteCore(ReadOnlySpan<char> value, bool appendNewLine)
    {
        var state = GetLineBufferState();
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
        ObfuscateCompletePatterns(state, patterns);
        FlushSafeOutput(state, patterns, shouldBuffer);
    }

    private LineBufferState GetLineBufferState()
    {
        var moduleType = ModuleLogger.CurrentModuleType.Value;
        var key = new LineBufferKey(moduleType, DirectWriteScope.Value);

        if (!_lineBuffers.TryGetValue(key, out var state))
        {
            state = new LineBufferState(moduleType);
            _lineBuffers.Add(key, state);
        }

        return state;
    }

    private static bool GetBufferMode(LineBufferState state, bool requestedBufferMode)
    {
        if (state.ShouldBuffer is null || state.Buffer.Length == 0)
        {
            state.ShouldBuffer = requestedBufferMode;
        }

        return state.ShouldBuffer.Value;
    }

    private string[] GetSecretPatterns()
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
        _secretPatterns = (snapshot.Secrets ?? [])
            .Where(pattern => !string.IsNullOrEmpty(pattern))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderByDescending(pattern => pattern.Length)
            .ToArray();
        _secretPatternsVersion = snapshot.Version;
        return _secretPatterns;
    }

    private void ObfuscateCompletePatterns(
        LineBufferState state,
        IReadOnlyList<string> patterns,
        bool preservePotentialLongerMatch = true)
    {
        if (state.Buffer.Length == 0 || patterns.Count == 0)
        {
            return;
        }

        var retainedPrefixLength = GetPotentialPatternPrefixLength(state.Buffer, patterns);
        var pending = state.Buffer.ToString();
        var output = new StringBuilder(pending.Length);
        var searchIndex = 0;
        var replaced = false;
        var retainedPrefixStart = pending.Length - retainedPrefixLength;

        while (searchIndex < pending.Length)
        {
            var match = FindFirstPattern(pending, patterns, searchIndex);
            if (match.Index < 0)
            {
                output.Append(pending, searchIndex, pending.Length - searchIndex);
                break;
            }

            if (preservePotentialLongerMatch
                && retainedPrefixLength > 0
                && match.Index + match.Length > retainedPrefixStart)
            {
                output.Append(pending, searchIndex, pending.Length - searchIndex);
                break;
            }

            output.Append(pending, searchIndex, match.Index - searchIndex);
            var secret = pending.Substring(match.Index, match.Length);
            output.Append(_secretObfuscator.Obfuscate(secret, null));
            searchIndex = match.Index + match.Length;
            replaced = true;
        }

        if (replaced)
        {
            state.Buffer.Clear();
            state.Buffer.Append(output);
        }
    }

    private void FlushSafeOutput(LineBufferState state, IReadOnlyList<string> patterns, bool shouldBuffer)
    {
        var retainedLength = patterns.Count == 0
            ? 0
            : GetPotentialPatternPrefixLength(state.Buffer, patterns);

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
        IReadOnlyList<string> patterns,
        int startIndex)
    {
        var firstIndex = -1;
        var longestMatch = 0;

        foreach (var pattern in patterns)
        {
            var index = input.IndexOf(pattern, startIndex, StringComparison.OrdinalIgnoreCase);
            if (index >= 0 && (firstIndex < 0 || index < firstIndex || (index == firstIndex && pattern.Length > longestMatch)))
            {
                firstIndex = index;
                longestMatch = pattern.Length;
            }
        }

        return (firstIndex, longestMatch);
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

        var output = state.Buffer.ToString(0, length);
        state.Buffer.Remove(0, length);
        _realConsole.Write(_secretObfuscator.Obfuscate(output, null));
    }

    private void WriteCompletedLine(string line, bool shouldBuffer, Type? moduleType)
    {
        var obfuscated = _secretObfuscator.Obfuscate(line, null);

        if (shouldBuffer)
        {
            RouteToBuffer(obfuscated, moduleType, appendNewLine: true);
        }
        else
        {
            _realConsole.WriteLine(obfuscated);
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

        var pending = state.Buffer.ToString(0, length);
        state.Buffer.Remove(0, length);
        var obfuscated = _secretObfuscator.Obfuscate(pending, null);

        if (shouldBuffer)
        {
            RouteToBuffer(obfuscated, state.ModuleType, appendNewLine: false);
        }
        else
        {
            _realConsole.Write(obfuscated);
        }
    }

    /// <inheritdoc />
    public override void Flush()
    {
        // Flush any partial line in the buffer
        lock (_lineBufferLock)
        {
            foreach (var state in _lineBuffers.Values.Where(state => state.Buffer.Length > 0))
            {
                var shouldBuffer = state.ShouldBuffer ?? ShouldBuffer();
                ObfuscateCompletePatterns(state, GetSecretPatterns());
                FlushSafePrefix(state, state.Buffer.Length, shouldBuffer);
                FlushPartialLine(state, shouldBuffer);
                state.ShouldBuffer = null;
            }
        }

        // Always flush real console (needed for Spectre.Console internals)
        _realConsole.Flush();
    }

    /// <summary>
    /// Flushes output that cannot be a prefix of a registered secret while retaining
    /// incomplete secret prefixes for subsequent writes.
    /// </summary>
    internal Task FlushAvailableAsync()
    {
        lock (_lineBufferLock)
        {
            var patterns = GetSecretPatterns();
            foreach (var state in _lineBuffers.Values.Where(state => state.Buffer.Length > 0))
            {
                var shouldBuffer = state.ShouldBuffer ?? ShouldBuffer();
                ObfuscateCompletePatterns(state, patterns, preservePotentialLongerMatch: false);
                FlushSafeOutput(state, patterns, shouldBuffer);

                if (shouldBuffer)
                {
                    var retainedLength = patterns.Length == 0
                        ? 0
                        : GetPotentialPatternPrefixLength(state.Buffer, patterns);
                    FlushPartialPrefix(state, state.Buffer.Length - retainedLength, shouldBuffer);
                }

                if (state.Buffer.Length == 0)
                {
                    state.ShouldBuffer = null;
                }
            }
        }

        return _realConsole.FlushAsync();
    }

    /// <inheritdoc />
    public override Task FlushAsync()
    {
        Flush();
        return _realConsole.FlushAsync();
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
        public Type? ModuleType { get; } = moduleType;

        public StringBuilder Buffer { get; } = new();

        public bool? ShouldBuffer { get; set; }
    }

    private readonly record struct LineBufferKey(Type? ModuleType, bool IsDirectWrite);

    private sealed class DirectWriteScopeRestorer(bool previousValue) : IDisposable
    {
        public void Dispose()
        {
            DirectWriteScope.Value = previousValue;
        }
    }
}
