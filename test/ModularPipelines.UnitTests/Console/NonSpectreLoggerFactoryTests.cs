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
        using var loggerFactory = CreateLoggerFactory(provider, options);
        var logger = CreateFactory(loggerFactory).CreateLoggers("Allowed.Category").Single();

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
        using var loggerFactory = CreateLoggerFactory(provider, options);
        var logger = CreateFactory(loggerFactory).CreateLoggers("Allowed.Category").Single();

        logger.LogInformation("delivered");

        await Assert.That(provider.Entries).HasSingleItem();
        await Assert.That(provider.Entries[0]).IsEqualTo((LogLevel.Information, "delivered"));
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
        using var loggerFactory = new LoggerFactory([suppressibleSpectreProvider]);
        var logger = new NonSpectreLoggerFactory(loggerFactory, suppression)
            .CreateLoggers("Category")
            .Single();

        loggerFactory.AddProvider(dynamicProvider);
        logger.LogWarning("delivered");

        await Assert.That(spectreProvider.Entries).IsEmpty();
        await Assert.That(dynamicProvider.Entries).HasSingleItem();
        await Assert.That(dynamicProvider.Entries[0]).IsEqualTo((LogLevel.Warning, "delivered"));
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

    private static NonSpectreLoggerFactory CreateFactory(ILoggerFactory loggerFactory)
    {
        return new NonSpectreLoggerFactory(loggerFactory, new SpectreLoggerSuppression());
    }

    private static LoggerFactory CreateLoggerFactory(
        ILoggerProvider provider,
        LoggerFilterOptions options)
    {
        var monitor = new Mock<IOptionsMonitor<LoggerFilterOptions>>();
        monitor.SetupGet(x => x.CurrentValue).Returns(options);
        monitor
            .Setup(x => x.OnChange(It.IsAny<Action<LoggerFilterOptions, string?>>()))
            .Returns(Mock.Of<IDisposable>());
        return new LoggerFactory([provider], monitor.Object);
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
