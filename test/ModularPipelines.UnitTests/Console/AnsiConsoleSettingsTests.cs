using ModularPipelines.Console;
using Spectre.Console;

namespace ModularPipelines.UnitTests.Console;

public class AnsiConsoleSettingsTests
{
    [Test]
    public async Task Known_Build_Agents_Keep_Ansi_Output_When_Redirected()
    {
        using var writer = new StringWriter();
        var settings = ConsoleCoordinator.CreateAnsiConsoleSettings(writer, isKnownBuildAgent: true);

        using (Assert.Multiple())
        {
            await Assert.That(settings.Ansi).IsEqualTo(AnsiSupport.Yes);
            await Assert.That(settings.ColorSystem).IsEqualTo(ColorSystemSupport.Standard);
            await Assert.That(settings.Interactive).IsEqualTo(InteractionSupport.No);
        }

        var console = AnsiConsole.Create(settings);
        console.Markup("[red]colored[/]");

        await Assert.That(writer.ToString()).Contains("\u001B[");
    }
}
