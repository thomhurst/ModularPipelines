using System.Collections.Concurrent;

namespace ModularPipelines.Logging;

/// <summary>
/// Buffers exception messages for deferred output at the end of pipeline execution.
/// Prevents exceptions from being lost when they occur during module disposal or cleanup.
/// </summary>
/// <remarks>
/// This buffer uses thread-safe operations to collect exception messages from multiple modules
/// executing in parallel. Messages are formatted and output using an injected formatter
/// that implements the chosen output strategy (console, file, structured logging, etc.).
/// </remarks>
internal class ExceptionBuffer : IExceptionBuffer
{
    private readonly IExceptionOutputFormatter _outputFormatter;
    private readonly ConcurrentQueue<string> _exceptionMessages = new();

    public bool HasExceptions => !_exceptionMessages.IsEmpty;

    public ExceptionBuffer(IExceptionOutputFormatter outputFormatter)
    {
        _outputFormatter = outputFormatter;
    }

    public void AddExceptionMessage(string message)
    {
        _exceptionMessages.Enqueue(message);
    }

    public void FlushExceptions()
    {
        if (_exceptionMessages.IsEmpty)
        {
            return;
        }

        var messages = new List<string>();
        while (_exceptionMessages.TryDequeue(out var message))
        {
            messages.Add(message);
        }

        _outputFormatter.FormatAndOutput(messages);
    }
}
