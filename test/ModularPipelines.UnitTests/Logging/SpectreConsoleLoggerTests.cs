using MEL.Spectre;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using Moq;
using Spectre.Console;

namespace ModularPipelines.UnitTests.Logging;

public class SpectreConsoleLoggerTests
{
    [Test]
    public async Task Configuration_Uses_Synchronous_Output()
    {
        var options = new SpectreConsoleLoggerOptions();

        DependencyInjectionSetup.ConfigureSpectreConsoleLogger(options, AnsiConsole.Console);

        await Assert.That(options.WriteMode).IsEqualTo(WriteMode.Synchronous);
    }

    [Test]
    public async Task Terminates_Each_Log_Entry_Once()
    {
        var writer = new StringWriter();
        await using var provider = CreateProvider(writer, AnsiSupport.No, ColorSystemSupport.NoColors);
        var logger = provider.GetRequiredService<ILogger<SpectreConsoleLoggerTests>>();

        logger.LogInformation("One log entry");

        var output = writer.ToString();
        await Assert.That(output).Contains("One log entry");
        await Assert.That(output).EndsWith(Environment.NewLine);
        await Assert.That(output).DoesNotContain(Environment.NewLine + Environment.NewLine);
    }

    [Test]
    public async Task Leaves_Only_Command_Output_Unstyled()
    {
        var options = new SpectreConsoleLoggerOptions();

        DependencyInjectionSetup.ConfigureSpectreConsoleLogger(options);

        await Assert.That(options.Theme.Placeholders.Resolve("CommandOutput", "build output"))
            .IsEqualTo(Style.Plain);
        await Assert.That(options.Theme.Placeholders.Resolve("CommandError", "build error"))
            .IsEqualTo(Style.Plain);
        await Assert.That(options.Theme.Placeholders.Resolve("Output", "build output"))
            .IsNotEqualTo(Style.Plain);
        await Assert.That(options.Theme.Placeholders.Resolve("Error", "build error"))
            .IsNotEqualTo(Style.Plain);
    }

    [Test]
    public async Task Logs_Inline_Output_With_Command_Output_Placeholder()
    {
        var logger = new CapturingModuleLogger();
        var loggerProvider = new Mock<IModuleLoggerProvider>();
        loggerProvider.Setup(x => x.GetLogger()).Returns(logger);
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()))
            .Returns((string? value, object? _) => value ?? string.Empty);
        var commandLogger = new CommandLogger(
            loggerProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            secretObfuscator.Object);

        commandLogger.Log(
            null,
            new CommandExecutionOptions
            {
                LogSettings = new CommandLoggingOptions
                {
                    Verbosity = CommandLogVerbosity.Normal,
                    ShowCommandArguments = true,
                    ShowStandardOutput = true,
                },
            },
            "tool --version",
            0,
            null,
            "short output",
            string.Empty,
            "C:\\repo");

        await Assert.That(logger.Properties["CommandOutput"]?.ToString()).Contains("short output");
        await Assert.That(logger.Properties["CommandMessage"]?.ToString()).DoesNotContain("short output");
    }

    [Test]
    public async Task Renders_Markup_Level_And_Exception_Details()
    {
        var writer = new StringWriter();
        await using var provider = CreateProvider(writer, AnsiSupport.Yes, ColorSystemSupport.Standard);
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

    private static ServiceProvider CreateProvider(
        StringWriter writer,
        AnsiSupport ansiSupport,
        ColorSystemSupport colorSystemSupport)
    {
        var console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Ansi = ansiSupport,
            ColorSystem = colorSystemSupport,
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

        return services.BuildServiceProvider();
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

    private sealed class CapturingModuleLogger : IModuleLogger
    {
        public IReadOnlyDictionary<string, object?> Properties { get; private set; }
            = new Dictionary<string, object?>();

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (state is not IEnumerable<KeyValuePair<string, object?>> properties)
            {
                throw new InvalidOperationException("Expected structured log state.");
            }

            Properties = properties.ToDictionary(x => x.Key, x => x.Value);
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public void Dispose()
        {
        }
    }
}
