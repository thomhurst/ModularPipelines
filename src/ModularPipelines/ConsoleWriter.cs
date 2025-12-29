using System.Diagnostics.CodeAnalysis;
using Spectre.Console;

namespace ModularPipelines;

[ExcludeFromCodeCoverage]
internal class ConsoleWriter : IConsoleWriter
{
    public void LogToConsole(string value)
    {
        try
        {
            AnsiConsole.MarkupLine(value);
        }
        catch (InvalidOperationException)
        {
            // Fall back to plain console output if markup parsing fails
            // (e.g., unbalanced or invalid markup characters)
            Console.WriteLine(value);
        }
    }
}
