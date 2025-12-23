using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine;

namespace ModularPipelines.Logging;

/// <summary>
/// Consolidated implementation for module output handling.
/// Manages buffering, CI-specific formatting, and section headers with duration/status.
/// </summary>
/// <remarks>
/// Uses a global static lock to ensure module outputs don't interleave.
/// Each module's output is flushed as an atomic block when the module completes.
/// </remarks>
internal class ModuleOutputWriter : IModuleOutputWriter, IDisposable
{
    /// <summary>
    /// Global lock to ensure only one module can flush its output at a time.
    /// This prevents output from multiple modules being interwoven.
    /// </summary>
    private static readonly object FlushLock = new();

    private readonly IServiceScope _scope;
    private readonly ILogEventBuffer _buffer;
    private readonly IBuildSystemFormatter _formatter;
    private readonly IConsoleWriter _consoleWriter;
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly ModuleOutputContext _context;
    private readonly object _writeLock = new();
    private bool _isDisposed;

    public ModuleOutputWriter(
        IServiceScope scope,
        IBuildSystemFormatterProvider formatterProvider,
        IConsoleWriter consoleWriter,
        ISecretObfuscator secretObfuscator,
        string moduleName)
    {
        _scope = scope;
        _buffer = scope.ServiceProvider.GetRequiredService<ILogEventBuffer>();
        _formatter = formatterProvider.GetFormatter();
        _consoleWriter = consoleWriter;
        _secretObfuscator = secretObfuscator;
        _context = new ModuleOutputContext(moduleName);
    }

    /// <inheritdoc />
    public void WriteLine(string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        lock (_writeLock)
        {
            if (_isDisposed)
            {
                return;
            }

            var obfuscated = _secretObfuscator.Obfuscate(message, null) ?? message;
            _buffer.Add(obfuscated);
        }
    }

    /// <inheritdoc />
    /// <remarks>
    /// This method is not thread-safe and does not use internal locking.
    /// Callers should ensure single-threaded access or provide their own synchronization.
    /// Intended for use in scenarios where the caller controls execution order (e.g., critical messages during long operations).
    /// </remarks>
    public void WriteLineDirect(string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        var obfuscated = _secretObfuscator.Obfuscate(message, null) ?? message;
        _consoleWriter.LogToConsole(obfuscated);
    }

    /// <inheritdoc />
    public IDisposable BeginSection(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        return new OutputSection(this, name);
    }

    /// <summary>
    /// Sets the exception for failed module status in section header.
    /// This method is for internal framework use only and is called by the module execution engine
    /// when a module fails to record the exception for display in the output section header.
    /// </summary>
    /// <param name="exception">The exception that caused the module to fail.</param>
    internal void SetException(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        _context.Exception = exception;
    }

    /// <summary>
    /// Flushes all buffered output with section header/footer.
    /// </summary>
    public void Dispose()
    {
        lock (_writeLock)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
        }

        if (!_buffer.HasEvents)
        {
            return;
        }

        lock (FlushLock)
        {
            var sectionHeader = _context.FormatSectionHeader();

            // Start collapsible section
            var startCommand = _formatter.GetStartBlockCommand(sectionHeader);
            if (startCommand != null)
            {
                _consoleWriter.LogToConsole(startCommand);
            }

            // Flush all buffered content
            var events = _buffer.GetAndClear();
            foreach (var evt in events)
            {
                if (evt.IsString && evt.StringValue != null)
                {
                    _consoleWriter.LogToConsole(evt.StringValue);
                }
            }

            // End collapsible section
            var endCommand = _formatter.GetEndBlockCommand(sectionHeader);
            if (endCommand != null)
            {
                _consoleWriter.LogToConsole(endCommand);
            }
        }

        _scope.Dispose();
        GC.SuppressFinalize(this);
    }

    ~ModuleOutputWriter()
    {
        Dispose();
    }

    /// <summary>
    /// Represents a nested output section within module output.
    /// </summary>
    private sealed class OutputSection : IDisposable
    {
        private readonly ModuleOutputWriter _writer;
        private readonly string _name;

        public OutputSection(ModuleOutputWriter writer, string name)
        {
            _writer = writer;
            _name = name;

            var startCommand = _writer._formatter.GetStartBlockCommand(name);
            if (startCommand != null)
            {
                _writer._buffer.Add(startCommand);
            }
        }

        public void Dispose()
        {
            var endCommand = _writer._formatter.GetEndBlockCommand(_name);
            if (endCommand != null)
            {
                _writer._buffer.Add(endCommand);
            }
        }
    }
}
