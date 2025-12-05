using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Helpers;
using Spectre.Console;

namespace ModularPipelines.Logging;

/// <summary>
/// Formats and outputs exception messages using Spectre.Console for rich terminal output.
/// Provides colored, formatted exception display with separators and headers.
/// </summary>
/// <remarks>
/// This implementation uses Spectre.Console's markup capabilities to provide:
/// - Warning icon and colored "Deferred Exceptions" header
/// - Blank lines for visual separation
/// - Direct output to AnsiConsole for immediate display.
/// </remarks>
[ExcludeFromCodeCoverage]
internal class SpectreExceptionFormatter : IExceptionOutputFormatter
{
    public void FormatAndOutput(IEnumerable<string> exceptionMessages)
    {
        var messagesList = exceptionMessages.ToList();

        if (!messagesList.Any())
        {
            return;
        }

        // Add separator and header
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"{MarkupFormatter.WarningIcon} [bold red]Deferred Exceptions[/]");
        AnsiConsole.WriteLine();

        // Output all exception messages
        foreach (var message in messagesList)
        {
            AnsiConsole.WriteLine(message);
        }

        AnsiConsole.WriteLine();
    }
}
