using MEL.Spectre;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.DependencyInjection;
using Spectre.Console;

namespace ModularPipelines.UnitTests.Logging;

public class SpectreConsoleLoggerTests
{
    [Test]
    public async Task Renders_Markup_Level_And_Exception_Details()
    {
        var writer = new StringWriter();
        var console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Ansi = AnsiSupport.Yes,
            ColorSystem = ColorSystemSupport.Standard,
            Interactive = InteractionSupport.No,
            Out = new AnsiConsoleOutput(writer),
        });

        var services = new ServiceCollection();
        services.AddLogging(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Trace);
            builder.AddSpectreConsole(options =>
            {
                DependencyInjectionSetup.ConfigureSpectreConsoleLogger(options, console);
                options.CiMode = CiMode.Off;
                options.WriteMode = WriteMode.Synchronous;
            });
        });

        await using var provider = services.BuildServiceProvider();
        var logger = provider.GetRequiredService<ILogger<SpectreConsoleLoggerTests>>();
        var exception = CaptureException();

        logger.LogError(exception, "Failure in [bold]pipeline[/]");

        var output = writer.ToString();
        await Assert.That(output).Contains("FAIL");
        await Assert.That(output).Contains("Failure in");
        await Assert.That(output).Contains("pipeline");
        await Assert.That(output).Contains(nameof(InvalidOperationException));
        await Assert.That(output).Contains("logger failure");
        await Assert.That(output).Contains(nameof(CaptureException));
        await Assert.That(output).Contains("\u001b[");
        await Assert.That(output).DoesNotContain("[bold]");
    }

    private static Exception CaptureException()
    {
        try
        {
            throw new InvalidOperationException("logger failure");
        }
        catch (Exception exception)
        {
            return exception;
        }
    }
}
