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

    private readonly ILogEventBuffer _buffer;
    private readonly IBuildSystemFormatter _formatter;
    private readonly IConsoleWriter _consoleWriter;
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly ModuleOutputContext _context;
    private readonly object _writeLock = new();
    private bool _isDisposed;

    public ModuleOutputWriter(
        ILogEventBuffer buffer,
        IBuildSystemFormatterProvider formatterProvider,
        IConsoleWriter consoleWriter,
        ISecretObfuscator secretObfuscator,
        string moduleName)
    {
        _buffer = buffer;
        _formatter = formatterProvider.GetFormatter();
        _consoleWriter = consoleWriter;
        _secretObfuscator = secretObfuscator;
        _context = new ModuleOutputContext(moduleName);
    }

    /// <inheritdoc />
    public void WriteLine(string message)
    {
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
    public void WriteLineDirect(string message)
    {
        var obfuscated = _secretObfuscator.Obfuscate(message, null) ?? message;
        _consoleWriter.LogToConsole(obfuscated);
    }

    /// <inheritdoc />
    public IDisposable BeginSection(string name)
    {
        return new OutputSection(this, name);
    }

    /// <summary>
    /// Sets the exception for failed module status in section header.
    /// </summary>
    public void SetException(Exception exception)
    {
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
