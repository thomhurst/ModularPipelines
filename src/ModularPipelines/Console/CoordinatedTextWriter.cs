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
    private readonly IConsoleCoordinator _coordinator;
    private readonly TextWriter _realConsole;
    private readonly Func<bool> _shouldBuffer;
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly ISecretProvider _secretProvider;
    private readonly StringBuilder _lineBuffer = new();
    private readonly object _lineBufferLock = new();
    private bool? _lineBufferShouldBuffer;

    /// <summary>
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
    private void RouteToBuffer(string message)
    {
        var currentModule = ModuleLogger.CurrentModuleType.Value;

        if (currentModule != null)
        {
            // Inside a module - route to that module's buffer
            var buffer = _coordinator.GetModuleBuffer(currentModule);
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
        var requestedBufferMode = _shouldBuffer();
        string[]? patterns = null;
        var segmentStart = 0;
        var newlineIndex = value.IndexOf('\n', segmentStart);

        while (newlineIndex >= 0)
        {
            var shouldBuffer = GetBufferMode(requestedBufferMode);
            _lineBuffer.Append(value, segmentStart, newlineIndex - segmentStart);

            if (!shouldBuffer)
            {
                FlushSafeDirectOutput(patterns ??= GetSecretPatterns());
            }

            WriteCompletedLine(shouldBuffer);
            segmentStart = newlineIndex + 1;
            newlineIndex = value.IndexOf('\n', segmentStart);
        }

        var finalBufferMode = GetBufferMode(requestedBufferMode);
        _lineBuffer.Append(value, segmentStart, value.Length - segmentStart);

        if (!finalBufferMode)
        {
            FlushSafeDirectOutput(patterns ??= GetSecretPatterns());
        }

        if (appendNewLine)
        {
            WriteCompletedLine(finalBufferMode);
        }
    }

    private bool GetBufferMode(bool requestedBufferMode)
    {
        if (_lineBufferShouldBuffer is null || _lineBuffer.Length == 0)
        {
            _lineBufferShouldBuffer = requestedBufferMode;
        }

        return _lineBufferShouldBuffer.Value;
    }

    private string[] GetSecretPatterns() =>
        _secretProvider.Secrets
            .Where(pattern => !string.IsNullOrEmpty(pattern))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

    private void FlushSafeDirectOutput(IReadOnlyList<string> patterns)
    {
        if (patterns.Count == 0)
        {
            FlushDirectPrefix(_lineBuffer.Length);
            return;
        }

        while (_lineBuffer.Length > 0)
        {
            var pending = _lineBuffer.ToString();
            var match = FindFirstPattern(pending, patterns);
            if (match.Index >= 0)
            {
                if (match.Index == 0 && GetPotentialPatternPrefixLength(pending, patterns) == pending.Length)
                {
                    return;
                }

                FlushDirectPrefix(match.Index > 0 ? match.Index : match.Length);
                continue;
            }

            var retainedLength = GetPotentialPatternPrefixLength(pending, patterns);
            FlushDirectPrefix(pending.Length - retainedLength);
            return;
        }
    }

    private static (int Index, int Length) FindFirstPattern(string input, IReadOnlyList<string> patterns)
    {
        var firstIndex = -1;
        var longestMatch = 0;

        foreach (var pattern in patterns)
        {
            var index = input.IndexOf(pattern, StringComparison.OrdinalIgnoreCase);
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

    private void FlushDirectPrefix(int length)
    {
        if (length <= 0)
        {
            return;
        }

        var output = _lineBuffer.ToString(0, length);
        _lineBuffer.Remove(0, length);
        _realConsole.Write(_secretObfuscator.Obfuscate(output, null));
    }

    private void WriteCompletedLine(bool shouldBuffer)
    {
        var line = _lineBuffer.ToString().TrimEnd('\r');
        _lineBuffer.Clear();
        _lineBufferShouldBuffer = null;
        var obfuscated = _secretObfuscator.Obfuscate(line, null);

        if (shouldBuffer)
        {
            RouteToBuffer(obfuscated);
        }
        else
        {
            _realConsole.WriteLine(obfuscated);
        }
    }

    private void FlushPartialLine(bool shouldBuffer)
    {
        if (_lineBuffer.Length == 0)
        {
            return;
        }

        var pending = _lineBuffer.ToString();
        _lineBuffer.Clear();
        var obfuscated = _secretObfuscator.Obfuscate(pending, null);

        if (shouldBuffer)
        {
            RouteToBuffer(obfuscated);
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
            if (_lineBuffer.Length > 0)
            {
                FlushPartialLine(_lineBufferShouldBuffer ?? _shouldBuffer());
                _lineBufferShouldBuffer = null;
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
}
