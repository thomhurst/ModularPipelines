using System.Collections.Concurrent;

namespace ModularPipelines.Logging;

internal class ExceptionBuffer : IExceptionBuffer
{
    private readonly ConcurrentQueue<string> _exceptionMessages = new();

    public bool HasExceptions => !_exceptionMessages.IsEmpty;

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

        // Add a separator to clearly indicate deferred exceptions
        Console.WriteLine();
        Console.WriteLine("========== Deferred Exceptions ==========");
        Console.WriteLine();

        while (_exceptionMessages.TryDequeue(out var message))
        {
            Console.WriteLine(message);
        }

        Console.WriteLine();
    }
}