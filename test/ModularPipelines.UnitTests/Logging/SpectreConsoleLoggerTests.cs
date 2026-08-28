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
    public async Task Configuration_Uses_Registered_Console()
    {
        var console = Mock.Of<IAnsiConsole>();
        var services = new ServiceCollection();
        DependencyInjectionSetup.Initialize(services);
        services.AddSingleton(console);
        await using var provider = services.BuildServiceProvider();

        var options = provider
            .GetRequiredService<Microsoft.Extensions.Options.IOptions<SpectreConsoleLoggerOptions>>()
            .Value;

        await Assert.That(options.Console).IsSameReferenceAs(console);
    }

    [Test]
    public async Task Configuration_Uses_Synchronous_Output()
    {
        var options = new SpectreConsoleLoggerOptions();

        DependencyInjectionSetup.ConfigureSpectreConsoleLogger(options, AnsiConsole.Console);

        await Assert.That(options.WriteMode).IsEqualTo(WriteMode.Synchronous);
    }

    [Test]
    public async Task Configuration_Disables_External_Scope_And_Activity_Rendering()
    {
        var options = new SpectreConsoleLoggerOptions();

        DependencyInjectionSetup.ConfigureSpectreConsoleLogger(options, AnsiConsole.Console);

        await Assert.That(options.IncludeScopes).IsFalse();
        await Assert.That(options.IncludeActivity).IsFalse();
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
        var loggerAccessor = new Mock<IModuleLoggerAccessor>();
        loggerAccessor.Setup(x => x.Logger).Returns(logger);
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()))
            .Returns((string? value, object? _) => value ?? string.Empty);
        var commandLogger = new CommandLogger(
            loggerAccessor.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            secretObfuscator.Object);

        commandLogger.Log(
            null,
            new CommandExecutionOptions
            {
                Logging = new CommandLoggingOptions
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

        var outputEntry = logger.Entries.Single(properties => properties.ContainsKey("CommandOutput"));
        await Assert.That(outputEntry["CommandOutput"]?.ToString()).Contains("short output");
        await Assert.That(outputEntry.Values.Select(x => x?.ToString())).DoesNotContain("tool --version");
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
        public List<IReadOnlyDictionary<string, object?>> Entries { get; } = [];

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

            Entries.Add(properties.ToDictionary(x => x.Key, x => x.Value));
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public void Dispose()
        {
        }
    }
}
