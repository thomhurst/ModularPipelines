using System.Text.Json;
using MEL.Spectre;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using ModularPipelines.Console;
using Moq;

namespace ModularPipelines.UnitTests.Console;

[TUnit.Core.NotInParallel]
public class NonSpectreLoggerFactoryTests
{
    [Test]
    public async Task FilterRulesAllowOverlappingWildcardPrefixAndSuffix()
    {
        var options = new LoggerFilterOptions { MinLevel = LogLevel.Error };
        options.Rules.Add(new LoggerFilterRule(
            null,
            "AB*BA",
            LogLevel.Information,
            null));

        var isEnabled = LoggerFilterRuleEvaluator.IsEnabled(
            options,
            typeof(RecordingLoggerProvider),
            "ABA",
            LogLevel.Information);

        await Assert.That(isEnabled).IsTrue();
    }

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
        var factory = CreateFactory(loggerFactory, control.Object, options);
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
        var factory = CreateFactory(loggerFactory, control.Object);
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
        var factory = CreateFactory(
            effectiveFactory,
            CreateControl().Object);

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
        var factory = CreateFactory(loggerFactory, CreateControl().Object);
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
        var control = new NoopSpectreConsoleLoggerControl(
            NullLoggerFactory.Instance,
            CreateOptionsMonitor(options));

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
            loggerFactory,
            serviceProvider.GetRequiredService<IOptionsMonitor<LoggerFilterOptions>>());

        using (Assert.Multiple())
        {
            await Assert.That(loggerFactory.CreateLogger("Category").IsEnabled(LogLevel.Information)).IsTrue();
            await Assert.That(control.WouldRender("Category", LogLevel.Information)).IsFalse();
            await Assert.That(control.WouldRender("Category", LogLevel.Warning)).IsTrue();
        }
    }

    [Test]
    public async Task NoopControlDetectsConsoleProviderAddedAfterConstruction()
    {
        var targetServices = new ServiceCollection();
        targetServices.AddLogging(builder => builder.ClearProviders());
        await using var targetServiceProvider = targetServices.BuildServiceProvider();
        var loggerFactory = targetServiceProvider.GetRequiredService<ILoggerFactory>();
        var control = new NoopSpectreConsoleLoggerControl(
            loggerFactory,
            targetServiceProvider.GetRequiredService<IOptionsMonitor<LoggerFilterOptions>>());

        var consoleServices = new ServiceCollection();
        consoleServices.AddLogging(builder => builder.ClearProviders().AddConsole());
        await using var consoleServiceProvider = consoleServices.BuildServiceProvider();
        var consoleProvider = consoleServiceProvider.GetServices<ILoggerProvider>()
            .OfType<ConsoleLoggerProvider>()
            .Single();

        await Assert.That(control.WouldRender("Category", LogLevel.Information)).IsFalse();

        loggerFactory.AddProvider(consoleProvider);

        await Assert.That(control.WouldRender("Category", LogLevel.Information)).IsTrue();
    }

    [Test]
    public async Task CreateLoggers_Uses_Configured_Console_Formatter_Synchronously()
    {
        var services = new ServiceCollection();
        services.AddLogging(builder => builder
            .ClearProviders()
            .AddJsonConsole());
        services.Configure<ConsoleLoggerOptions>(options =>
            options.LogToStandardErrorThreshold = LogLevel.Warning);
        await using var serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var filterOptions = serviceProvider.GetRequiredService<IOptionsMonitor<LoggerFilterOptions>>();
        var control = new NoopSpectreConsoleLoggerControl(
            loggerFactory,
            filterOptions);
        var factory = new NonSpectreLoggerFactory(
            loggerFactory,
            control,
            filterOptions);
        var originalOut = System.Console.Out;
        var originalError = System.Console.Error;
        var outputWriter = new StringWriter();
        var errorWriter = new StringWriter();

        try
        {
#pragma warning disable TUnit0055 // Globally non-parallel test restores both writers in finally.
            System.Console.SetOut(outputWriter);
            System.Console.SetError(errorWriter);
#pragma warning restore TUnit0055

            factory.CreateLoggers("Category").Single().LogWarning("formatted");

            using var document = JsonDocument.Parse(errorWriter.ToString());
            using (Assert.Multiple())
            {
                await Assert.That(outputWriter.ToString()).IsEmpty();
                await Assert.That(document.RootElement.GetProperty("LogLevel").GetString())
                    .IsEqualTo("Warning");
                await Assert.That(document.RootElement.GetProperty("Message").GetString())
                    .IsEqualTo("formatted");
            }
        }
        finally
        {
#pragma warning disable TUnit0055 // Restore the process-wide writers before leaving the test.
            System.Console.SetOut(originalOut);
            System.Console.SetError(originalError);
#pragma warning restore TUnit0055
        }
    }

    [Test]
    public async Task CreateLoggers_ClearsFormatterBufferAfterFailure()
    {
        var services = new ServiceCollection();
        services.AddLogging(builder => builder
            .ClearProviders()
            .AddConsole(options => options.FormatterName = FailingOnceConsoleFormatter.FormatterName)
            .AddConsoleFormatter<FailingOnceConsoleFormatter, ConsoleFormatterOptions>());
        await using var serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var filterOptions = serviceProvider.GetRequiredService<IOptionsMonitor<LoggerFilterOptions>>();
        var control = new NoopSpectreConsoleLoggerControl(loggerFactory, filterOptions);
        var logger = new NonSpectreLoggerFactory(loggerFactory, control, filterOptions)
            .CreateLoggers("Category")
            .Single();
        var originalOut = System.Console.Out;
        var outputWriter = new StringWriter();

        try
        {
#pragma warning disable TUnit0055 // Globally non-parallel test restores the writer in finally.
            System.Console.SetOut(outputWriter);
#pragma warning restore TUnit0055

            _ = Assert.Throws<InvalidOperationException>(() => logger.LogInformation("first"));
            logger.LogInformation("second");

            await Assert.That(outputWriter.ToString()).IsEqualTo("clean");
        }
        finally
        {
#pragma warning disable TUnit0055 // Restore the process-wide writer before leaving the test.
            System.Console.SetOut(originalOut);
#pragma warning restore TUnit0055
        }
    }

    [Test]
    public async Task CreateLoggers_UsesOptionsFromProviderAddedAfterConstruction()
    {
        var targetServices = new ServiceCollection();
        targetServices.AddLogging(builder => builder.ClearProviders());
        await using var targetServiceProvider = targetServices.BuildServiceProvider();
        var loggerFactory = targetServiceProvider.GetRequiredService<ILoggerFactory>();
        var filterOptions = targetServiceProvider
            .GetRequiredService<IOptionsMonitor<LoggerFilterOptions>>();
        var control = new NoopSpectreConsoleLoggerControl(loggerFactory, filterOptions);
        var factory = new NonSpectreLoggerFactory(loggerFactory, control, filterOptions);

        var providerServices = new ServiceCollection();
        providerServices.AddLogging(builder => builder
            .ClearProviders()
            .AddJsonConsole());
        providerServices.Configure<ConsoleLoggerOptions>(options =>
            options.LogToStandardErrorThreshold = LogLevel.Warning);
        await using var providerServiceProvider = providerServices.BuildServiceProvider();
        var consoleProvider = providerServiceProvider.GetServices<ILoggerProvider>()
            .OfType<ConsoleLoggerProvider>()
            .Single();
        loggerFactory.AddProvider(consoleProvider);

        var originalOut = System.Console.Out;
        var originalError = System.Console.Error;
        var outputWriter = new StringWriter();
        var errorWriter = new StringWriter();

        try
        {
#pragma warning disable TUnit0055 // Globally non-parallel test restores both writers in finally.
            System.Console.SetOut(outputWriter);
            System.Console.SetError(errorWriter);
#pragma warning restore TUnit0055

            factory.CreateLoggers("Category").Single().LogWarning("external provider");

            using var document = JsonDocument.Parse(errorWriter.ToString());
            using (Assert.Multiple())
            {
                await Assert.That(outputWriter.ToString()).IsEmpty();
                await Assert.That(document.RootElement.GetProperty("LogLevel").GetString())
                    .IsEqualTo("Warning");
                await Assert.That(document.RootElement.GetProperty("Message").GetString())
                    .IsEqualTo("external provider");
            }
        }
        finally
        {
#pragma warning disable TUnit0055 // Restore the process-wide writers before leaving the test.
            System.Console.SetOut(originalOut);
            System.Console.SetError(originalError);
#pragma warning restore TUnit0055
        }
    }

    [Test]
    public async Task CreateLoggers_Includes_Provider_Added_After_Construction()
    {
        var services = new ServiceCollection();
        services.AddLogging(builder => builder
            .ClearProviders()
            .AddConsole());
        await using var serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var filterOptions = serviceProvider.GetRequiredService<IOptionsMonitor<LoggerFilterOptions>>();
        var control = new NoopSpectreConsoleLoggerControl(
            loggerFactory,
            filterOptions);
        var factory = new NonSpectreLoggerFactory(
            loggerFactory,
            control,
            filterOptions);
        var runtimeProvider = new RecordingLoggerProvider();

        loggerFactory.AddProvider(runtimeProvider);
        foreach (var logger in factory.CreateLoggers("Category"))
        {
            logger.LogInformation("dynamic provider message");
        }

        await Assert.That(runtimeProvider.Entries)
            .Contains((LogLevel.Information, "dynamic provider message"));
    }

    private static Mock<ISpectreConsoleLoggerControl> CreateControl()
    {
        var control = new Mock<ISpectreConsoleLoggerControl>();
        control.Setup(x => x.Suspend()).Returns(Mock.Of<IDisposable>());
        return control;
    }

    private static IOptionsMonitor<T> CreateOptionsMonitor<T>(T options)
    {
        var monitor = new Mock<IOptionsMonitor<T>>();
        monitor.SetupGet(x => x.CurrentValue).Returns(options);
        monitor.Setup(x => x.Get(It.IsAny<string?>())).Returns(options);
        monitor
            .Setup(x => x.OnChange(It.IsAny<Action<T, string?>>()))
            .Returns(Mock.Of<IDisposable>());
        return monitor.Object;
    }

    private static NonSpectreLoggerFactory CreateFactory(
        ILoggerFactory loggerFactory,
        ISpectreConsoleLoggerControl control,
        LoggerFilterOptions? options = null) => new(
        loggerFactory,
        control,
        CreateOptionsMonitor(options ?? new LoggerFilterOptions()));

    private sealed class FailingOnceConsoleFormatter()
        : ConsoleFormatter(FormatterName)
    {
        public const string FormatterName = "failing-once";

        private int _callCount;

        public override void Write<TState>(
            in LogEntry<TState> logEntry,
            IExternalScopeProvider? scopeProvider,
            TextWriter textWriter)
        {
            if (Interlocked.Increment(ref _callCount) == 1)
            {
                textWriter.Write("stale");
                throw new InvalidOperationException("formatter failed");
            }

            textWriter.Write("clean");
        }
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
