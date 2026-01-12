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
    private readonly Func<bool> _isProgressActive;
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly StringBuilder _lineBuffer = new();
    private readonly object _lineBufferLock = new();

    /// <summary>
    /// Initializes a new coordinated text writer.
    /// </summary>
    /// <param name="coordinator">The console coordinator.</param>
    /// <param name="realConsole">The real console to write to when not buffering.</param>
    /// <param name="isProgressActive">Function that returns whether progress is active.</param>
    /// <param name="secretObfuscator">Obfuscator for secrets in output.</param>
    public CoordinatedTextWriter(
        IConsoleCoordinator coordinator,
        TextWriter realConsole,
        Func<bool> isProgressActive,
        ISecretObfuscator secretObfuscator)
    {
        _coordinator = coordinator;
        _realConsole = realConsole;
        _isProgressActive = isProgressActive;
        _secretObfuscator = secretObfuscator;
    }

    /// <inheritdoc />
    public override Encoding Encoding => _realConsole.Encoding;

    /// <inheritdoc />
    public override void WriteLine(string? value)
    {
        var message = value ?? string.Empty;
        var obfuscated = _secretObfuscator.Obfuscate(message, null);

        if (!_isProgressActive())
        {
            // Progress not running - write directly
            _realConsole.WriteLine(obfuscated);
            return;
        }

        // Progress is active - buffer the output
        RouteToBuffer(obfuscated);
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

        var obfuscated = _secretObfuscator.Obfuscate(value, null);

        if (!_isProgressActive())
        {
            // Progress not running - write directly
            _realConsole.Write(obfuscated);
            return;
        }

        // Progress is active - accumulate until newline
        lock (_lineBufferLock)
        {
            foreach (var c in obfuscated)
            {
                if (c == '\n')
                {
                    // Flush the line
                    var line = _lineBuffer.ToString().TrimEnd('\r');
                    _lineBuffer.Clear();
                    RouteToBuffer(line);
                }
                else
                {
                    _lineBuffer.Append(c);
                }
            }
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

    /// <inheritdoc />
    public override void Flush()
    {
        // Flush any partial line in the buffer
        lock (_lineBufferLock)
        {
            if (_lineBuffer.Length > 0)
            {
                var line = _lineBuffer.ToString();
                _lineBuffer.Clear();

                if (_isProgressActive())
                {
                    RouteToBuffer(line);
                }
                else
                {
                    _realConsole.Write(line);
                }
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
