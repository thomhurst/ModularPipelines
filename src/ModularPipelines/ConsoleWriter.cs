using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Logging;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ModularPipelines;

[ExcludeFromCodeCoverage]
internal class ConsoleWriter : IConsoleWriter
{
    public void LogToConsole(string value)
    {
        if (ModuleLogger.Values.Value is IConsoleWriter moduleConsoleWriter)
        {
            moduleConsoleWriter.LogToConsole(value);
            return;
        }

        try
        {
            AnsiConsole.MarkupLine(value);
        }
        catch (InvalidOperationException)
        {
            // Fall back to plain console output if markup parsing fails
            // (e.g., unbalanced or invalid markup characters)
            System.Console.WriteLine(value);
        }
    }

    public void Write(IRenderable renderable)
    {
        if (ModuleLogger.Values.Value is IConsoleWriter moduleConsoleWriter)
        {
            moduleConsoleWriter.Write(renderable);
            return;
        }

        AnsiConsole.Write(renderable);
    }
}
