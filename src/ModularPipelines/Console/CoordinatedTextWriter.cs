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
    private static readonly object UnattributedContext = new();

    private readonly IConsoleCoordinator _coordinator;
    private readonly TextWriter _realConsole;
    private readonly Func<bool> _shouldBuffer;
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly ISecretProvider _secretProvider;
    private readonly Dictionary<object, LineBufferState> _lineBuffers = [];
    private readonly object _lineBufferLock = new();

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
        lock (_lineBufferLock)
        {
            WriteCore(value ?? string.Empty, appendNewLine: true);
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
            WriteCore(value, appendNewLine: false);
        }
    }

    /// <inheritdoc />
    public override void Write(char value)
    {
        Write(value.ToString());
    }

    /// <inheritdoc />
    public override void Write(char[] buffer, int index, int count)
    {
        Write(new string(buffer, index, count));
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

    private void WriteCore(string value, bool appendNewLine)
    {
        var state = GetLineBufferState();
        var shouldBuffer = GetBufferMode(state, _shouldBuffer());
        state.Buffer.Append(value);

        if (appendNewLine)
        {
            state.Buffer.Append(Environment.NewLine);
        }

        var patterns = GetSecretPatterns();
        ObfuscateCompletePatterns(state, patterns);
        FlushSafeOutput(state, patterns, shouldBuffer);
    }

    private LineBufferState GetLineBufferState()
    {
        var moduleType = ModuleLogger.CurrentModuleType.Value;
        var key = (object?)moduleType ?? UnattributedContext;

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

    private string[] GetSecretPatterns() =>
        _secretProvider.Secrets
            .Where(pattern => !string.IsNullOrEmpty(pattern))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderByDescending(pattern => pattern.Length)
            .ToArray();

    private void ObfuscateCompletePatterns(LineBufferState state, IReadOnlyList<string> patterns)
    {
        if (state.Buffer.Length == 0 || patterns.Count == 0)
        {
            return;
        }

        var pending = state.Buffer.ToString();
        var output = new StringBuilder(pending.Length);
        var searchIndex = 0;
        var replaced = false;

        while (searchIndex < pending.Length)
        {
            var match = FindFirstPattern(pending, patterns, searchIndex);
            if (match.Index < 0)
            {
                output.Append(pending, searchIndex, pending.Length - searchIndex);
                break;
            }

            var matchReachesEnd = match.Index + match.Length == pending.Length;
            if (matchReachesEnd
                && GetPotentialPatternPrefixLength(pending, patterns) == pending.Length - match.Index)
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
            : GetPotentialPatternPrefixLength(state.Buffer.ToString(), patterns);

        FlushSafePrefix(state, state.Buffer.Length - retainedLength, shouldBuffer);

        if (state.Buffer.Length == 0)
        {
            state.ShouldBuffer = null;
        }
    }

    private void FlushSafePrefix(LineBufferState state, int safeLength, bool shouldBuffer)
    {
        while (safeLength > 0)
        {
            var newlineIndex = state.Buffer.ToString(0, safeLength).IndexOf('\n');
            if (newlineIndex < 0)
            {
                break;
            }

            var line = state.Buffer.ToString(0, newlineIndex).TrimEnd('\r');
            state.Buffer.Remove(0, newlineIndex + 1);
            safeLength -= newlineIndex + 1;
            WriteCompletedLine(line, shouldBuffer, state.ModuleType);
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

    private static int GetPotentialPatternPrefixLength(string input, IReadOnlyList<string> patterns)
    {
        var maximumLength = patterns.Max(pattern => pattern.Length) - 1;
        for (var length = Math.Min(input.Length, maximumLength); length > 0; length--)
        {
            var suffix = input.AsSpan(input.Length - length, length);
            foreach (var pattern in patterns)
            {
                if (pattern.Length > length && pattern.AsSpan().StartsWith(suffix, StringComparison.OrdinalIgnoreCase))
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
            RouteToBuffer(obfuscated, moduleType);
        }
        else
        {
            _realConsole.WriteLine(obfuscated);
        }
    }

    private void FlushPartialLine(LineBufferState state, bool shouldBuffer)
    {
        if (state.Buffer.Length == 0)
        {
            return;
        }

        var pending = state.Buffer.ToString();
        state.Buffer.Clear();
        var obfuscated = _secretObfuscator.Obfuscate(pending, null);

        if (shouldBuffer)
        {
            RouteToBuffer(obfuscated, state.ModuleType);
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
                var shouldBuffer = state.ShouldBuffer ?? _shouldBuffer();
                ObfuscateCompletePatterns(state, GetSecretPatterns());
                FlushSafePrefix(state, state.Buffer.Length, shouldBuffer);
                FlushPartialLine(state, shouldBuffer);
                state.ShouldBuffer = null;
            }
        }

        // Always flush real console (needed for Spectre.Console internals)
        _realConsole.Flush();
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

    private sealed class LineBufferState(Type? moduleType)
    {
        public Type? ModuleType { get; } = moduleType;

        public StringBuilder Buffer { get; } = new();

        public bool? ShouldBuffer { get; set; }
    }
}
