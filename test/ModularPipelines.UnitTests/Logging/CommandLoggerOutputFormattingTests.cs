using Microsoft.Extensions.Logging;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using ModularPipelines.Secrets;
using Moq;

namespace ModularPipelines.UnitTests.Logging;

public class CommandLoggerOutputFormattingTests
{
    [Test]
    public async Task TruncationWarningNamesTheCaptureLimitAndBothStreams()
    {
        var (commandLogger, logger) = CreateCommandLogger();

        commandLogger.LogOutputTruncation(null, null, 6, 2, 10);

        var warning = logger.Messages.Single(message => message.Level == LogLevel.Warning);
        await Assert.That(warning.Text).Contains("MaxCapturedOutputLength (10 characters per stream)");
        await Assert.That(warning.Text).Contains("6 standard output characters and 2 standard error characters");
        await Assert.That(warning.Text).Contains("set it to 0 for unlimited capture");
    }

    [Test]
    [Arguments(CommandLogVerbosity.Silent, null, false)]
    [Arguments(CommandLogVerbosity.Normal, CommandLogVerbosity.Silent, false)]
    [Arguments(CommandLogVerbosity.Silent, CommandLogVerbosity.Normal, true)]
    public async Task TruncationWarningHonorsEffectiveVerbosity(CommandLogVerbosity pipelineVerbosity, CommandLogVerbosity? commandVerbosity, bool expectWarning)
    {
        var (commandLogger, logger) = CreateCommandLogger(new CommandLoggingOptions { Verbosity = pipelineVerbosity });
        var executionOptions = new CommandExecutionOptions
        {
            Logging = commandVerbosity is { } verbosity ? new CommandLoggingOptions { Verbosity = verbosity } : null,
        };

        commandLogger.LogOutputTruncation(null, executionOptions, 6, 2, 10);

        await Assert.That(logger.Messages.Any(message => message.Level == LogLevel.Warning)).IsEqualTo(expectWarning);
    }
    [Test]
    public async Task CapturedOutput_PrefixesEveryLine_AndSkipsBlankLines()
    {
        var (commandLogger, logger) = CreateCommandLogger();

        Log(commandLogger, exitCode: 0, standardOutput: "first\r\n\r\n   \nsecond  \nthird", standardError: string.Empty);

        var outputLines = logger.Messages
            .Where(message => message.Level == LogLevel.Information && message.Text.StartsWith("  ↳", StringComparison.Ordinal))
            .Select(message => message.Text)
            .ToList();

        using (Assert.Multiple())
        {
            await Assert.That(outputLines).IsEquivalentTo(["  ↳ first", "  ↳ second", "  ↳ third"]);
            await Assert.That(logger.Messages.Any(message => message.Text.Contains('→'))).IsFalse();
        }
    }

    [Test]
    public async Task ShortCapturedOutput_UsesSameMarkerAsStreamedOutput()
    {
        var (commandLogger, logger) = CreateCommandLogger();

        Log(commandLogger, exitCode: 0, standardOutput: "1.2.3", standardError: string.Empty);

        await Assert.That(logger.Messages.Select(message => message.Text)).Contains("  ↳ 1.2.3");
    }

    [Test]
    public async Task CapturedError_IsOneWarning_WithAlignedContinuationLines()
    {
        var (commandLogger, logger) = CreateCommandLogger();

        Log(commandLogger, exitCode: 1, standardOutput: string.Empty, standardError: "boom\n\nat line 2\n");

        var warning = logger.Messages.Single(message => message.Level == LogLevel.Warning);
        await Assert.That(warning.Text).IsEqualTo($"  ✗ boom{Environment.NewLine}    at line 2");
    }

    [Test]
    public async Task StreamedOutput_SkipsBlankLines()
    {
        var (commandLogger, logger) = CreateCommandLogger();
        ICommandOutputLogger outputLogger = commandLogger;
        var toolOptions = new CommandLineToolOptions("tool");
        var executionOptions = CreateExecutionOptions();

        outputLogger.LogStandardOutputLine(toolOptions, executionOptions, "Build succeeded.");
        outputLogger.LogStandardOutputLine(toolOptions, executionOptions, string.Empty);
        outputLogger.LogStandardOutputLine(toolOptions, executionOptions, "   ");
        outputLogger.LogStandardOutputLine(toolOptions, executionOptions, "    0 Error(s)   ");

        await Assert.That(logger.Messages.Select(message => message.Text))
            .IsEquivalentTo(["  ↳ Build succeeded.", "  ↳     0 Error(s)"]);
    }

    private static void Log(CommandLogger commandLogger, int exitCode, string standardOutput, string standardError) =>
        commandLogger.LogCommandCompletion(
            null,
            CreateExecutionOptions(),
            "tool run",
            exitCode,
            null,
            standardOutput,
            standardError,
            "/repo");

    private static CommandExecutionOptions CreateExecutionOptions() => new()
    {
        Logging = new CommandLoggingOptions
        {
            Verbosity = CommandLogVerbosity.Normal,
            ShowCommandArguments = true,
            ShowStandardOutput = true,
            ShowStandardError = true,
        },
    };

    private static (CommandLogger CommandLogger, CollectingLogger Logger) CreateCommandLogger(CommandLoggingOptions? pipelineLogging = null)
    {
        var logger = new CollectingLogger();
        var loggerAccessor = new Mock<IModuleLoggerAccessor>();
        loggerAccessor.Setup(x => x.Logger).Returns(logger);
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()))
            .Returns((string? value, object? _) => value ?? string.Empty);
        var commandLogger = new CommandLogger(
            loggerAccessor.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions { Commands = new PipelineCommandOptions { Logging = pipelineLogging } }),
            secretObfuscator.Object);
        return (commandLogger, logger);
    }

    private sealed record LoggedMessage(LogLevel Level, string Text);

    private sealed class CollectingLogger : ILogger
    {
        public List<LoggedMessage> Messages { get; } = [];

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter) =>
            Messages.Add(new LoggedMessage(logLevel, formatter(state, exception)));
    }
}
