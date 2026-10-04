using Microsoft.Extensions.Logging;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using ModularPipelines.Secrets;
using Moq;

namespace ModularPipelines.UnitTests.Logging;

public class CommandLoggerOutputFormattingTests
{
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

    [Test]
    [Arguments(CommandLogVerbosity.Detailed, false)]
    [Arguments(CommandLogVerbosity.Diagnostic, true)]
    public async Task DiagnosticAddsWorkingDirectoryAndTimestamps(CommandLogVerbosity verbosity, bool expected)
    {
        var (commandLogger, logger) = CreateCommandLogger();
        commandLogger.LogCommandStart(null, new CommandExecutionOptions
        {
            Logging = new CommandLoggingOptions { Verbosity = verbosity },
        }, "tool run", "/repo");

        var message = logger.Messages.Single().Text;
        await Assert.That(message.Contains("/repo", StringComparison.Ordinal)).IsEqualTo(expected);
        await Assert.That(System.Text.RegularExpressions.Regex.IsMatch(message, @"^\[\d{4}-\d{2}-\d{2}T")).IsEqualTo(expected);
    }

    [Test]
    public async Task ExplicitFalseOverridesDiagnosticDefaults()
    {
        var (commandLogger, logger) = CreateCommandLogger();
        commandLogger.Log(null, new CommandExecutionOptions
        {
            Logging = CommandLoggingOptions.Diagnostic with
            {
                ShowCommandArguments = false,
                ShowStandardOutput = false,
                ShowStandardError = false,
                ShowExitCode = false,
                ShowExecutionTime = false,
                ShowWorkingDirectory = false,
                ShowTimestamps = false,
            },
        }, "private-input", 7, TimeSpan.FromSeconds(12), "private-output", "private-error", "/private-directory");

        var messages = string.Join("\n", logger.Messages.Select(message => message.Text));
        await Assert.That(messages).DoesNotContain("private-");
        await Assert.That(messages).DoesNotContain("exit ");
        await Assert.That(messages).DoesNotContain("12s");
        await Assert.That(messages).DoesNotContain("[");
    }

    [Test]
    public async Task ExplicitOutputOverridesSilentForCapturedAndStreamedOutput()
    {
        var (commandLogger, logger) = CreateCommandLogger();
        var execution = new CommandExecutionOptions
        {
            Logging = CommandLoggingOptions.Silent with
            {
                ShowStandardOutput = true,
                ShowStandardError = true,
            },
        };
        commandLogger.Log(null, execution, "hidden-input", 0, null, "captured-output", "captured-error", "/hidden-directory");
        ((ICommandOutputLogger) commandLogger).LogStandardOutputLine(new CommandLineToolOptions("tool"), execution, "streamed-output");
        ((ICommandOutputLogger) commandLogger).LogStandardErrorLine(new CommandLineToolOptions("tool"), execution, "streamed-error");

        var messages = string.Join("\n", logger.Messages.Select(message => message.Text));
        await Assert.That(messages).Contains("captured-output");
        await Assert.That(messages).Contains("captured-error");
        await Assert.That(messages).Contains("streamed-output");
        await Assert.That(messages).Contains("streamed-error");
        await Assert.That(messages).DoesNotContain("hidden-");
    }

    [Test]
    public async Task SuccessfulCapturedStandardErrorUsesInformationWithoutWarningAnnotations()
    {
        var (commandLogger, logger) = CreateCommandLogger();
        commandLogger.LogCommandCompletion(null, new CommandExecutionOptions
        {
            Logging = CommandLoggingOptions.Silent with { ShowStandardError = true },
        }, "tool run", 0, null, string.Empty, "progress one\n\nprogress two", "/repo");

        await Assert.That(logger.Messages.Select(message => message.Text))
            .IsEquivalentTo(["  ↳ progress one", "  ↳ progress two"]);
        await Assert.That(logger.Messages.All(message => message.Level == LogLevel.Information)).IsTrue();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ExplicitTimestampsApplyToEveryCommandEvent(bool enabled)
    {
        var (commandLogger, logger) = CreateCommandLogger();
        var execution = new CommandExecutionOptions
        {
            Logging = new CommandLoggingOptions { ShowTimestamps = enabled },
        };
        commandLogger.Log(null, execution, "tool run", 1, TimeSpan.FromSeconds(1), "output", "error", "/repo");
        ((ICommandOutputLogger) commandLogger).LogStandardOutputLine(new CommandLineToolOptions("tool"), execution, "streamed");
        ((ICommandOutputLogger) commandLogger).LogStandardErrorLine(new CommandLineToolOptions("tool"), execution, "streamed error");

        await Assert.That(logger.Messages.Count).IsEqualTo(6);
        foreach (var message in logger.Messages)
        {
            await Assert.That(System.Text.RegularExpressions.Regex.IsMatch(message.Text, @"^\[\d{4}-\d{2}-\d{2}T")).IsEqualTo(enabled);
        }
    }

    [Test]
    public async Task DryRunHonorsDirectoryAndTimestampOverrides()
    {
        var (commandLogger, logger) = CreateCommandLogger();
        commandLogger.Log(null, new CommandExecutionOptions
        {
            InternalDryRun = true,
            Logging = CommandLoggingOptions.Silent with { ShowWorkingDirectory = true, ShowTimestamps = true },
        }, "hidden-input", 0, null, "hidden-output", "hidden-error", "/repo");

        var message = logger.Messages.Single().Text;
        await Assert.That(message).Contains("/repo> ******** [DRY-RUN]");
        await Assert.That(message).DoesNotContain("hidden-");
        await Assert.That(System.Text.RegularExpressions.Regex.IsMatch(message, @"^\[\d{4}-\d{2}-\d{2}T")).IsTrue();
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

    private static (CommandLogger CommandLogger, CollectingLogger Logger) CreateCommandLogger()
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
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
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
