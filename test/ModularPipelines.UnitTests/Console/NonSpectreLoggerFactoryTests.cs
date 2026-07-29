using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Console;
using Moq;

namespace ModularPipelines.UnitTests.Console;

public class NonSpectreLoggerFactoryTests
{
    [Test]
    public async Task CreateLoggers_Applies_Provider_And_Category_Rules()
    {
        var provider = new RecordingLoggerProvider();
        var options = new LoggerFilterOptions
        {
            MinLevel = LogLevel.Error,
        };
        options.Rules.Add(new LoggerFilterRule(
            "Recording",
            "Allowed",
            LogLevel.Warning,
            null));
        var logger = CreateFactory(provider, options).CreateLoggers("Allowed.Category").Single();

        logger.LogInformation("filtered");
        logger.LogWarning("delivered");

        await Assert.That(provider.Entries).HasSingleItem();
        await Assert.That(provider.Entries[0]).IsEqualTo((LogLevel.Warning, "delivered"));
    }

    [Test]
    public async Task CreateLoggers_Rule_With_No_Minimum_Overrides_Global_Minimum()
    {
        var provider = new RecordingLoggerProvider();
        var options = new LoggerFilterOptions
        {
            MinLevel = LogLevel.Error,
        };
        options.Rules.Add(new LoggerFilterRule(
            "Recording",
            "Allowed",
            null,
            (_, _, _) => true));
        var logger = CreateFactory(provider, options).CreateLoggers("Allowed.Category").Single();

        logger.LogInformation("delivered");

        await Assert.That(provider.Entries).HasSingleItem();
        await Assert.That(provider.Entries[0]).IsEqualTo((LogLevel.Information, "delivered"));
    }

    private static NonSpectreLoggerFactory CreateFactory(
        ILoggerProvider provider,
        LoggerFilterOptions options)
    {
        var monitor = new Mock<IOptionsMonitor<LoggerFilterOptions>>();
        monitor.SetupGet(x => x.CurrentValue).Returns(options);
        return new NonSpectreLoggerFactory([provider], monitor.Object);
    }

    [ProviderAlias("Recording")]
    private sealed class RecordingLoggerProvider : ILoggerProvider
    {
        public List<(LogLevel LogLevel, string Message)> Entries { get; } = [];

        public ILogger CreateLogger(string categoryName) => new RecordingLogger(Entries);

        public void Dispose()
        {
        }
    }

    private sealed class RecordingLogger(
        List<(LogLevel LogLevel, string Message)> entries) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
            => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            entries.Add((logLevel, formatter(state, exception)));
        }
    }
}
