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
        var options = new LoggerFilterOptions
        {
            MinLevel = LogLevel.Error,
        };
        options.Rules.Add(new LoggerFilterRule(
            "Recording",
            "Allowed",
            LogLevel.Warning,
            null));
        using var factory = CreateFactory(provider, options);
        var logger = factory.CreateLoggers("Allowed.Category").Single();

        logger.LogInformation("filtered");
        logger.LogWarning("delivered");

        await Assert.That(provider.Entries).HasSingleItem();
        await Assert.That(provider.Entries[0]).IsEqualTo((LogLevel.Warning, "delivered"));
    }

    [Test]
    public async Task CreateLoggers_Rule_With_No_Minimum_Overrides_Global_Minimum()
    {
        var provider = new RecordingLoggerProvider();
        string? filteredProviderName = null;
        var options = new LoggerFilterOptions
        {
            MinLevel = LogLevel.Error,
        };
        options.Rules.Add(new LoggerFilterRule(
            "Recording",
            "Allowed",
            null,
            (providerName, _, _) =>
            {
                filteredProviderName = providerName;
                return true;
            }));
        using var factory = CreateFactory(provider, options);
        var logger = factory.CreateLoggers("Allowed.Category").Single();

        logger.LogInformation("delivered");

        await Assert.That(provider.Entries).HasSingleItem();
        await Assert.That(provider.Entries[0]).IsEqualTo((LogLevel.Information, "delivered"));
        await Assert.That(filteredProviderName)
            .IsEqualTo(typeof(RecordingLoggerProvider).FullName);
    }

    [Test]
    public async Task CreateLoggers_Includes_Provider_Added_After_Logger_Creation()
    {
        var spectreProvider = new RecordingLoggerProvider();
        var dynamicProvider = new RecordingLoggerProvider();
        var suppression = new SpectreLoggerSuppression();
        using var suppressibleSpectreProvider =
            new SuppressibleSpectreLoggerProvider(
                spectreProvider,
                Mock.Of<ISpectreConsoleLoggerControl>(),
                suppression);
        var loggerFactory = new LoggerFactory([suppressibleSpectreProvider]);
        using var trackingFactory = new ProviderTrackingLoggerFactory(
            loggerFactory,
            [suppressibleSpectreProvider]);
        using var factory = new NonSpectreLoggerFactory(
            trackingFactory,
            CreateOptionsMonitor(new LoggerFilterOptions()));
        var logger = factory
            .CreateLoggers("Category")
            .Single();

        trackingFactory.AddProvider(dynamicProvider);
        logger.LogWarning("delivered");

        await Assert.That(spectreProvider.Entries).IsEmpty();
        await Assert.That(dynamicProvider.Entries).HasSingleItem();
        await Assert.That(dynamicProvider.Entries[0]).IsEqualTo((LogLevel.Warning, "delivered"));
    }

    [Test]
    public async Task CreateLoggers_ReportsOnlyFailedProviderForRetry()
    {
        var successfulProvider = new RecordingLoggerProvider();
        var failingProvider = new RecordingLoggerProvider
        {
            LogException = new InvalidOperationException("provider rejected event"),
        };
        using var factory = new NonSpectreLoggerFactory(
            new TestProviderRegistry([successfulProvider, failingProvider]),
            CreateOptionsMonitor(new LoggerFilterOptions()));
        var logger = factory.CreateLoggers("Category").Single();

        var exception = Assert.Throws<ProviderDeliveryException>(
            () => logger.LogWarning("delivered"));

        await Assert.That(successfulProvider.Entries).HasSingleItem();
        await Assert.That(failingProvider.Entries).IsEmpty();
        await Assert.That(exception.FailedLoggers).HasSingleItem();

        failingProvider.LogException = null;
        exception.FailedLoggers.Single().LogWarning("delivered");

        await Assert.That(successfulProvider.Entries).HasSingleItem();
        await Assert.That(failingProvider.Entries).HasSingleItem();
    }

    [Test]
    public async Task Registration_Resolves_Logger_Factory_And_Control()
    {
        var services = new ServiceCollection();
        services.AddLogging(builder =>
        {
            builder.AddSpectreConsole();
            builder.Services.MakeSpectreLoggerSuppressible();
        });
        await using var serviceProvider = services.BuildServiceProvider();

        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var control = serviceProvider.GetRequiredService<ISpectreConsoleLoggerControl>();

        await Assert.That(loggerFactory).IsNotNull();
        await Assert.That(control.SynchronizationLock).IsNotNull();
    }

    [Test]
    public async Task Filter_PostConfigure_Translates_Full_Provider_Name_In_Place()
    {
        string? filteredProviderName = null;
        Func<string?, string?, LogLevel, bool> filter = (providerName, _, _) =>
        {
            filteredProviderName = providerName;
            return true;
        };
        var options = new LoggerFilterOptions();
        options.Rules.Add(new LoggerFilterRule(
            "Other.Provider",
            "Before",
            LogLevel.Trace,
            null));
        options.Rules.Add(new LoggerFilterRule(
            SpectreLoggerSuppressionRegistration.SpectreProviderTypeName,
            "Category",
            LogLevel.Warning,
            filter));
        options.Rules.Add(new LoggerFilterRule(
            "Other.Provider",
            "After",
            LogLevel.Critical,
            null));

        new SpectreLoggerFilterOptionsPostConfigure().PostConfigure(null, options);

        var translatedRule = options.Rules[1];
        var filterResult = translatedRule.Filter!(
            translatedRule.ProviderName,
            translatedRule.CategoryName,
            translatedRule.LogLevel!.Value);
        using (Assert.Multiple())
        {
            await Assert.That(options.Rules[0].CategoryName).IsEqualTo("Before");
            await Assert.That(translatedRule.ProviderName)
                .IsEqualTo(typeof(SuppressibleSpectreLoggerProvider).FullName);
            await Assert.That(translatedRule.CategoryName).IsEqualTo("Category");
            await Assert.That(translatedRule.LogLevel).IsEqualTo(LogLevel.Warning);
            await Assert.That(filterResult).IsTrue();
            await Assert.That(filteredProviderName)
                .IsEqualTo(SpectreLoggerSuppressionRegistration.SpectreProviderTypeName);
            await Assert.That(options.Rules[2].CategoryName).IsEqualTo("After");
        }
    }

    private static NonSpectreLoggerFactory CreateFactory(
        ILoggerProvider provider,
        LoggerFilterOptions options)
    {
        return new NonSpectreLoggerFactory(
            new TestProviderRegistry([provider]),
            CreateOptionsMonitor(options));
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

    private sealed class TestProviderRegistry(
        IReadOnlyList<ILoggerProvider> providers) : ILoggerProviderRegistry
    {
        public IReadOnlyList<ILoggerProvider> Providers { get; } = providers;
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

    private sealed class RecordingLogger(
        RecordingLoggerProvider provider) : ILogger
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
