using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Options;

namespace ModularPipelines;

public class ModuleLogger<T> : ILogger<T>, IModuleLogger
{
    private readonly IOptions<PipelineOptions> _options;
    private readonly StringBuilder _stringBuilder = new();

    public ModuleLogger(IOptions<PipelineOptions> options)
    {
        _options = options;
    }
    public IDisposable BeginScope<TState>(TState state)
    {
        return new NoopDisposable();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _options.Value.LoggerOptions.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string>? formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }
        
        var message = string.Empty;
        
        if (formatter != null)
        {
            message += formatter(state, exception);
        }
        
        _stringBuilder.AppendLine($"{logLevel.ToString()}: [{typeof(T).Name}] {message}");
    }

    public string GetOutput()
    {
        var output = _stringBuilder.ToString();

        if (string.IsNullOrEmpty(output))
        {
            return $"[{typeof(T).Name}] No output.";
        }
        
        return output;
    }

    private class NoopDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}