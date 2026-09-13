using System.Globalization;
using System.Text.RegularExpressions;
using ModularPipelines.Engine;
using ModularPipelines.Options;
using Spectre.Console;

namespace ModularPipelines.UnitTests.Console;

public partial class LogoPrinterTests
{
    [Test]
    [Arguments(90, AnsiSupport.No)]
    [Arguments(120, AnsiSupport.No)]
    [Arguments(160, AnsiSupport.Yes)]
    [Arguments(240, AnsiSupport.Yes)]
    public async Task First_Log_After_Logo_Starts_At_Left_Margin(int width, AnsiSupport ansi)
    {
        using var output = new StringWriter(CultureInfo.InvariantCulture);
        var console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(output),
            Ansi = ansi,
            Interactive = InteractionSupport.No,
            ColorSystem = ColorSystemSupport.Standard,
        });
        console.Profile.Width = width;
        var options = Microsoft.Extensions.Options.Options.Create(new PipelineOptions());
        var printer = new LogoPrinter(options, console);

        printer.PrintLogo();
        console.WriteLine("[INFO] Build System: GitHubActions (detected from GITHUB_ACTIONS)");

        var text = AnsiControlSequencePattern().Replace(output.ToString(), string.Empty);
        await Assert.That(text).Contains("88b           d88");
        var logLine = text.Split('\n').Single(line => line.Contains("[INFO]", StringComparison.Ordinal));
        await Assert.That(logLine.TrimEnd('\r'))
            .IsEqualTo("[INFO] Build System: GitHubActions (detected from GITHUB_ACTIONS)");
    }

    [GeneratedRegex(@"\x1B\[[0-?]*[ -/]*[@-~]")]
    private static partial Regex AnsiControlSequencePattern();
}
