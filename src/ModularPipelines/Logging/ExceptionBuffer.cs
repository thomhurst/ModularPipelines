using System.Collections.Concurrent;
using Spectre.Console;

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
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold red]⚠ Deferred Exceptions[/]");
        AnsiConsole.WriteLine();

        while (_exceptionMessages.TryDequeue(out var message))
        {
            AnsiConsole.WriteLine(message);
        }

        AnsiConsole.WriteLine();
    }
}