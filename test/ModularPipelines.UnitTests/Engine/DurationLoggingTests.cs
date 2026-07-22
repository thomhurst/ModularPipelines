using System.Collections.Concurrent;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Console;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Engine;

public class DurationLoggingTests
{
    private const string DisplayDurationPattern = @"\d+(?:ms|s & \d+ms|m & \d+s|h & \d+m)";

    [Test]
    public async Task Module_Logs_Use_Consistent_Duration_And_Timestamp_Formats()
    {
        var moduleLogger = new RecordingLogger<SuccessfulModule>();
        var schedulerLogger = new RecordingLogger<ModuleScheduler>();
        var builder = TestPipelineHostBuilder.Create();
        builder.Services.AddSingleton<ILogger<SuccessfulModule>>(moduleLogger);
        builder.Services.AddSingleton<ILogger<ModuleScheduler>>(schedulerLogger);

        await builder
            .AddModule<SuccessfulModule>()
            .ExecutePipelineAsync();

        var startMessage = moduleLogger.Messages.Single(message => message.Contains("execution started at", StringComparison.Ordinal));
        var startTime = startMessage[(startMessage.LastIndexOf(' ') + 1)..];
        var isIso8601 = DateTimeOffset.TryParseExact(
            startTime,
            "O",
            CultureInfo.InvariantCulture,
            DateTimeStyles.RoundtripKind,
            out _);

        var successMessage = moduleLogger.Messages.Single(message => message.StartsWith("Module succeeded after", StringComparison.Ordinal));

        await Assert.That(isIso8601).IsTrue();
        await Assert.That(Regex.IsMatch(successMessage, $"^Module succeeded after {DisplayDurationPattern}$")).IsTrue();
        await Assert.That(schedulerLogger.Messages.Any(message => message.Contains("completed after", StringComparison.Ordinal))).IsFalse();
    }

    [Test]
    public async Task Module_Group_Header_Uses_Display_Duration_Format()
    {
        var buffer = new ModuleOutputBuffer(typeof(SuccessfulModule));
        var formatter = new CapturingFormatter();
        buffer.WriteLine("output");

        buffer.FlushTo(TextWriter.Null, formatter, NullLogger.Instance);

        await Assert.That(formatter.Header).IsNotNull();
        await Assert.That(Regex.IsMatch(
            formatter.Header!,
            $"^SuccessfulModule ✓ \\({DisplayDurationPattern}\\)$")).IsTrue();
    }

    private sealed class SuccessfulModule : Module
    {
        protected override Task ExecuteModuleAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class RecordingLogger<T> : ILogger<T>
    {
        public ConcurrentQueue<string> Messages { get; } = new();

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Messages.Enqueue(formatter(state, exception));
        }
    }

    private sealed class CapturingFormatter : IBuildSystemFormatter
    {
        public string? Header { get; private set; }

        public string? GetStartBlockCommand(string name)
        {
            Header = name;
            return null;
        }

        public string? GetEndBlockCommand(string name) => null;

        public string? GetMaskSecretCommand(string secret) => null;
    }
}
