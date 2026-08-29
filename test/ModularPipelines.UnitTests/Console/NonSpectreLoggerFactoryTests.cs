using MEL.Spectre;
using Microsoft.Extensions.DependencyInjection;
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
        var options = new LoggerFilterOptions { MinLevel = LogLevel.Error };
        options.Rules.Add(new LoggerFilterRule(
            typeof(RecordingLoggerProvider).FullName,
            "Allowed",
            LogLevel.Warning,
            null));
        using var loggerFactory = new LoggerFactory([provider], CreateOptionsMonitor(options));
        var control = CreateControl();
        var factory = new NonSpectreLoggerFactory(loggerFactory, control.Object);
        var logger = factory.CreateLoggers("Allowed.Category").Single();

        logger.LogInformation("filtered");
        logger.LogWarning("delivered");

        await Assert.That(provider.Entries).HasSingleItem();
        await Assert.That(provider.Entries[0]).IsEqualTo((LogLevel.Warning, "delivered"));
        control.Verify(x => x.Suspend(), Times.Exactly(2));
    }

    [Test]
    public async Task CreateLoggers_Includes_Provider_Added_After_Logger_Creation()
    {
        var initialProvider = new RecordingLoggerProvider();
        var dynamicProvider = new RecordingLoggerProvider();
        using var loggerFactory = new LoggerFactory([initialProvider]);
        var control = CreateControl();
        var factory = new NonSpectreLoggerFactory(loggerFactory, control.Object);
        var logger = factory.CreateLoggers("Category").Single();

        loggerFactory.AddProvider(dynamicProvider);
        logger.LogWarning("delivered");

        await Assert.That(initialProvider.Entries).HasSingleItem();
        await Assert.That(dynamicProvider.Entries).HasSingleItem();
    }

    [Test]
    public async Task CreateLoggers_Uses_Effective_Logger_Factory()
    {
        var effectiveProvider = new RecordingLoggerProvider();
        using var effectiveFactory = new LoggerFactory([effectiveProvider]);
        var factory = new NonSpectreLoggerFactory(effectiveFactory, CreateControl().Object);

        factory.CreateLoggers("Category").Single().LogWarning("delivered");

        await Assert.That(effectiveProvider.Entries).HasSingleItem();
    }

    [Test]
    public async Task CreateLoggers_Wraps_Provider_Failure()
    {
        var failingProvider = new RecordingLoggerProvider
        {
            LogException = new InvalidOperationException("provider rejected event"),
        };
        using var loggerFactory = new LoggerFactory([failingProvider]);
        var factory = new NonSpectreLoggerFactory(loggerFactory, CreateControl().Object);
        var logger = factory.CreateLoggers("Category").Single();

        var exception = Assert.Throws<ProviderDeliveryException>(
            () => logger.LogWarning("delivered"));

        await Assert.That(exception.FailedLoggers).IsEmpty();
        await Assert.That(exception.InnerExceptions).HasSingleItem();
    }

    [Test]
    public async Task Registration_Resolves_Public_Logger_Control()
    {
        var services = new ServiceCollection();
        services.AddLogging(builder =>
        {
            builder.AddFilter("Filtered.Category", LogLevel.Warning);
            builder.AddSpectreConsole();
        });
        await using var serviceProvider = services.BuildServiceProvider();

        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var control = serviceProvider.GetRequiredService<ISpectreConsoleLoggerControl>();
        using var suspension = control.Suspend();

        using (Assert.Multiple())
        {
            await Assert.That(loggerFactory).IsNotNull();
            await Assert.That(control.SynchronizationLock).IsNotNull();
            await Assert.That(suspension).IsNotNull();
            await Assert.That(control.WouldRender("Filtered.Category", LogLevel.Information)).IsFalse();
            await Assert.That(control.WouldRender("Filtered.Category", LogLevel.Warning)).IsTrue();
        }
    }

    [Test]
    public async Task NoopControlDoesNotReportNonConsoleProvider()
    {
        var provider = new RecordingLoggerProvider();
        var options = new LoggerFilterOptions { MinLevel = LogLevel.Error };
        var control = new NoopSpectreConsoleLoggerControl(CreateOptionsMonitor(options), [provider]);

        using (Assert.Multiple())
        {
            await Assert.That(control.WouldRender("Category", LogLevel.Information)).IsFalse();
            await Assert.That(control.WouldRender("Category", LogLevel.Error)).IsFalse();
        }
    }

    [Test]
    public async Task NoopControlUsesConsoleProviderSpecificFilter()
    {
        var telemetryProvider = new RecordingLoggerProvider();
        var services = new ServiceCollection();
        services.AddLogging(builder =>
        {
            builder.AddProvider(telemetryProvider);
            builder.AddConsole();
            builder.AddFilter<Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider>(
                "Category",
                LogLevel.Warning);
            builder.AddFilter<RecordingLoggerProvider>("Category", LogLevel.Information);
        });
        await using var serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var control = new NoopSpectreConsoleLoggerControl(
            serviceProvider.GetRequiredService<IOptionsMonitor<LoggerFilterOptions>>(),
            serviceProvider.GetServices<ILoggerProvider>());

        using (Assert.Multiple())
        {
            await Assert.That(loggerFactory.CreateLogger("Category").IsEnabled(LogLevel.Information)).IsTrue();
            await Assert.That(control.WouldRender("Category", LogLevel.Information)).IsFalse();
            await Assert.That(control.WouldRender("Category", LogLevel.Warning)).IsTrue();
        }
    }

    private static Mock<ISpectreConsoleLoggerControl> CreateControl()
    {
        var control = new Mock<ISpectreConsoleLoggerControl>();
        control.Setup(x => x.Suspend()).Returns(Mock.Of<IDisposable>());
        return control;
    }

    private static IOptionsMonitor<LoggerFilterOptions> CreateOptionsMonitor(
        LoggerFilterOptions options)
    {
        var monitor = new Mock<IOptionsMonitor<LoggerFilterOptions>>();
        monitor.SetupGet(x => x.CurrentValue).Returns(options);
        monitor.Setup(x => x.Get(It.IsAny<string?>())).Returns(options);
        monitor
            .Setup(x => x.OnChange(It.IsAny<Action<LoggerFilterOptions, string?>>()))
            .Returns(Mock.Of<IDisposable>());
        return monitor.Object;
    }

    [ProviderAlias("Recording")]
    private sealed class RecordingLoggerProvider : ILoggerProvider
    {
        public List<(LogLevel LogLevel, string Message)> Entries { get; } = [];

        public Exception? LogException { get; set; }

        public ILogger CreateLogger(string categoryName) => new RecordingLogger(this);

        public void Dispose()
        {
        }
    }

    private sealed class RecordingLogger(RecordingLoggerProvider provider) : ILogger
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
            if (provider.LogException is not null)
            {
                throw provider.LogException;
            }

            provider.Entries.Add((logLevel, formatter(state, exception)));
        }
    }
}
