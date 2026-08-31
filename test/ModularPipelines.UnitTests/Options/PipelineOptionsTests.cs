using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;
using Spectre.Console;

namespace ModularPipelines.UnitTests.Options;

[NotInParallel]
public class PipelineOptionsTests
{
    private sealed class TestLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => NullLogger.Instance;

        public void Dispose()
        {
        }
    }

    private sealed class OptionsTestModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    private sealed class RegistrationOrderOptions
    {
        public List<string> Calls { get; } = [];
    }

    [Test]
    [Arguments(typeof(PipelineBuilderSettings))]
    [Arguments(typeof(PipelineOptions))]
    [Arguments(typeof(PipelineConsoleOptions))]
    [Arguments(typeof(PipelineHttpOptions))]
    [Arguments(typeof(PipelineCommandOptions))]
    [Arguments(typeof(ConcurrencyOptions))]
    [Arguments(typeof(HttpLoggingOptions))]
    [Arguments(typeof(HttpResilienceOptions))]
    [Arguments(typeof(SecretMaskingOptions))]
    public async Task PublicProperties_AreInitOnly(Type optionsType)
    {
        var mutableProperties = optionsType
            .GetProperties()
            .Where(property => property.SetMethod is not null)
            .Where(property => !property.SetMethod!.ReturnParameter
                .GetRequiredCustomModifiers()
                .Contains(typeof(IsExternalInit)))
            .Select(property => property.Name);

        await Assert.That(mutableProperties).IsEmpty();
    }

    [Test]
    public async Task SubsystemSettings_AreGrouped()
    {
        var pipelineOptionsType = typeof(PipelineOptions);
        var legacyPropertyNames = new HashSet<string>
        {
            "ShowProgressInConsole",
            "PrintResults",
            "PrintLogo",
            "PrintDependencyChains",
            "ConsoleWidth",
            "ModuleOutputFlushInterval",
            "ModuleOutputFlushThreshold",
            "DefaultHttpLoggingOptions",
            "DefaultHttpTimeout",
            "DefaultHttpResilienceOptions",
            "DefaultLoggingOptions",
            "DefaultExecutionOptions",
        };

        using (Assert.Multiple())
        {
            await Assert.That(pipelineOptionsType.GetProperty(nameof(PipelineOptions.Console))!.PropertyType)
                .IsEqualTo(typeof(PipelineConsoleOptions));
            await Assert.That(pipelineOptionsType.GetProperty(nameof(PipelineOptions.Http))!.PropertyType)
                .IsEqualTo(typeof(PipelineHttpOptions));
            await Assert.That(pipelineOptionsType.GetProperty(nameof(PipelineOptions.Commands))!.PropertyType)
                .IsEqualTo(typeof(PipelineCommandOptions));
            await Assert.That(pipelineOptionsType.GetProperties()
                    .Select(property => property.Name)
                    .Where(legacyPropertyNames.Contains))
                .IsEmpty();
        }
    }

    [Test]
    public async Task PipelineBuilderSettings_IsASealedRecord()
    {
        var original = new PipelineBuilderSettings { ApplicationName = "original" };
        var updated = original with { ApplicationName = "updated" };

        using (Assert.Multiple())
        {
            await Assert.That(typeof(PipelineBuilderSettings).IsSealed).IsTrue();
            await Assert.That(original.ApplicationName).IsEqualTo("original");
            await Assert.That(updated.ApplicationName).IsEqualTo("updated");
        }
    }

    [Test]
    public async Task ConfigureOptionsReplacesEarlierFilters()
    {
        var builder = Pipeline.CreateBuilder();

        builder.ConfigureOptions(options => options with { RunOnlyCategories = ["first"] });
        builder.ConfigureOptions(options => options with { RunOnlyCategories = ["second"] });
        builder.ConfigureOptions(options => options with { IgnoreCategories = ["ignored-first"] });
        builder.ConfigureOptions(options => options with { IgnoreCategories = ["ignored-second"] });

        using (Assert.Multiple())
        {
            await Assert.That(builder.Options.RunOnlyCategories).IsEquivalentTo(["second"]);
            await Assert.That(builder.Options.IgnoreCategories).IsEquivalentTo(["ignored-second"]);
            await Assert.That(typeof(PipelineBuilder).GetMethod("RunOnlyCategories")).IsNull();
            await Assert.That(typeof(PipelineBuilder).GetMethod("IgnoreCategories")).IsNull();
            await Assert.That(typeof(PipelineBuilder).GetMethod("SetLogLevel")).IsNull();
        }
    }

    [Test]
    public async Task InitializingProgressOption_DoesNotMutateSpectre()
    {
        var originalInteractive = AnsiConsole.Profile.Capabilities.Interactive;

        try
        {
            _ = new PipelineOptions
            {
                Console = new PipelineConsoleOptions
                {
                    ShowProgress = !originalInteractive,
                },
            };

            await Assert.That(AnsiConsole.Profile.Capabilities.Interactive)
                .IsEqualTo(originalInteractive);
        }
        finally
        {
            AnsiConsole.Profile.Capabilities.Interactive = originalInteractive;
        }
    }

    [Test]
    public async Task DefaultProgressOption_UsesSpectreCapability()
    {
        await Assert.That(new PipelineOptions().Console.ShowProgress)
            .IsEqualTo(AnsiConsole.Profile.Capabilities.Interactive);
    }

    [Test]
    public async Task CategoryFilters_AreDefensivelyCopied()
    {
        var runOnlyCategories = new List<string> { "run" };
        var ignoreCategories = new List<string> { "ignore" };
        var options = new PipelineOptions
        {
            RunOnlyCategories = runOnlyCategories,
            IgnoreCategories = ignoreCategories,
        };

        runOnlyCategories.Add("added later");
        ignoreCategories.Clear();

        using (Assert.Multiple())
        {
            await Assert.That(options.RunOnlyCategories).IsEquivalentTo(["run"]);
            await Assert.That(options.IgnoreCategories).IsEquivalentTo(["ignore"]);
            await Assert.That(((ICollection<string>) options.RunOnlyCategories!).IsReadOnly)
                .IsTrue();
            await Assert.That(((ICollection<string>) options.IgnoreCategories!).IsReadOnly)
                .IsTrue();
        }
    }

    [Test]
    public async Task NestedHttpCollections_AreDefensivelyCopied()
    {
        var sensitiveHeaders = new List<string> { "X-Secret" };
        var retryableStatusCodes = new List<HttpStatusCode>
        {
            HttpStatusCode.ServiceUnavailable,
        };
        var options = new PipelineOptions
        {
            Http = new PipelineHttpOptions
            {
                Logging = new HttpLoggingOptions
                {
                    SensitiveHeaderNames = sensitiveHeaders,
                },
                Resilience = new HttpResilienceOptions
                {
                    RetryableStatusCodes = retryableStatusCodes,
                },
            },
        };

        sensitiveHeaders.Clear();
        retryableStatusCodes.Add(HttpStatusCode.BadGateway);

        using (Assert.Multiple())
        {
            await Assert.That(options.Http.Logging!.SensitiveHeaderNames)
                .IsEquivalentTo(["X-Secret"]);
            await Assert.That(options.Http.Resilience!.RetryableStatusCodes)
                .IsEquivalentTo([HttpStatusCode.ServiceUnavailable]);
            await Assert.That(
                    ((ICollection<string>) options.Http.Logging.SensitiveHeaderNames)
                    .IsReadOnly)
                .IsTrue();
            await Assert.That(
                    ((ICollection<HttpStatusCode>)
                        options.Http.Resilience.RetryableStatusCodes)
                    .IsReadOnly)
                .IsTrue();
        }
    }

    [Test]
    public async Task PipelineBuilder_RegistersFinalOptionsSnapshot()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<OptionsTestModule>();
        var expected = builder.Options;

        await using var pipeline = await builder.BuildAsync();
        var options = pipeline.Services
            .GetRequiredService<IOptions<PipelineOptions>>()
            .Value;
        var snapshot = pipeline.Services
            .GetRequiredService<IOptionsSnapshot<PipelineOptions>>()
            .Value;
        var monitor = pipeline.Services
            .GetRequiredService<IOptionsMonitor<PipelineOptions>>()
            .CurrentValue;

        using (Assert.Multiple())
        {
            await Assert.That(options).IsEqualTo(expected);
            await Assert.That(options).IsNotSameReferenceAs(expected);
            await Assert.That(snapshot).IsSameReferenceAs(options);
            await Assert.That(monitor).IsSameReferenceAs(options);
        }
    }

    [Test]
    public async Task PipelineBuilder_RegistersSnapshotBackedOptionsFactory()
    {
        var builder = TestPipelineBuilder.Create()
            .ConfigureOptions(options => options with { DryRun = true })
            .AddModule<OptionsTestModule>();

        await using var pipeline = await builder.BuildAsync();
        var factory = pipeline.Services.GetRequiredService<IOptionsFactory<PipelineOptions>>();
        var first = factory.Create(Microsoft.Extensions.Options.Options.DefaultName);
        var second = factory.Create(Microsoft.Extensions.Options.Options.DefaultName);

        using (Assert.Multiple())
        {
            await Assert.That(first.DryRun).IsTrue();
            await Assert.That(second.DryRun).IsTrue();
            await Assert.That(first).IsNotSameReferenceAs(second);
        }
    }

    [Test]
    public async Task ConfigureOptionsRejectsNullResult()
    {
        var builder = Pipeline.CreateBuilder();

        var exception = Assert.Throws<InvalidOperationException>(
            () => builder.ConfigureOptions(_ => null!));

        await Assert.That(exception.Message)
            .IsEqualTo("The pipeline options configuration returned null.");
    }

    [Test]
    public async Task PipelineBuilderExposesLoggingBuilder()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<OptionsTestModule>();
        var loggerProvider = new TestLoggerProvider();
        var descriptor = ServiceDescriptor.Singleton<ILoggerProvider>(loggerProvider);

        builder.Logging.Services.Add(descriptor);
        await using var pipeline = await builder.BuildAsync();

        using (Assert.Multiple())
        {
            await Assert.That(builder.Logging.Services).Contains(descriptor);
            await Assert.That(pipeline.Services.GetServices<ILoggerProvider>())
                .Contains(loggerProvider);
        }
    }

    [Test]
    public async Task ClearingLoggingServicesPreservesApplicationServices()
    {
        var builder = Pipeline.CreateBuilder();
        var applicationService = ServiceDescriptor.Singleton(new object());
        builder.Services.Add(applicationService);

        builder.Logging.Services.Clear();

        await Assert.That(builder.Services).Contains(applicationService);
    }

    [Test]
    public async Task LoggingServicesHonorListOrderingOperations()
    {
        var services = Pipeline.CreateBuilder().Logging.Services;
        var first = ServiceDescriptor.Singleton(new object());
        var inserted = ServiceDescriptor.Singleton(new Uri("https://example.com"));
        var replacement = ServiceDescriptor.Singleton(TimeProvider.System);

        services.Insert(0, first);
        services.Insert(1, inserted);
        services[1] = replacement;

        using (Assert.Multiple())
        {
            await Assert.That(services[0]).IsSameReferenceAs(first);
            await Assert.That(services[1]).IsSameReferenceAs(replacement);
        }
    }

    [Test]
    public async Task PipelineBuilderServicesAllowTryAddLoggingReplacements()
    {
        var loggerProvider = new TestLoggerProvider();
        var builder = Pipeline.CreateBuilder()
            .AddModule<OptionsTestModule>();
        builder.Services.TryAddSingleton<ILoggerFactory>(NullLoggerFactory.Instance);
        builder.Services.TryAddSingleton<ILoggerProvider>(loggerProvider);

        await using var pipeline = await builder.BuildAsync();

        using (Assert.Multiple())
        {
            await Assert.That(pipeline.Services.GetRequiredService<ILoggerFactory>())
                .IsSameReferenceAs(NullLoggerFactory.Instance);
            await Assert.That(pipeline.Services.GetServices<ILoggerProvider>())
                .Contains(loggerProvider);
        }
    }

    [Test]
    public async Task PipelineBuilderLoggingTryAddHonorsApplicationServiceReplacement()
    {
        var replacement = new Uri("https://replacement.example");
        var builder = Pipeline.CreateBuilder()
            .AddModule<OptionsTestModule>();
        builder.Services.AddSingleton(replacement);
        builder.Logging.Services.TryAddSingleton(new Uri("https://default.example"));

        await using var pipeline = await builder.BuildAsync();

        await Assert.That(pipeline.Services.GetRequiredService<Uri>())
            .IsSameReferenceAs(replacement);
    }

    [Test]
    public async Task PipelineBuilderPreservesRegistrationOrderAcrossServiceViews()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<OptionsTestModule>();
        builder.Logging.Services.Configure<RegistrationOrderOptions>(options =>
            options.Calls.Add("logging-first"));
        builder.Services.Configure<RegistrationOrderOptions>(options =>
            options.Calls.Add("application-second"));
        builder.Logging.Services.Configure<RegistrationOrderOptions>(options =>
            options.Calls.Add("logging-third"));

        await using var pipeline = await builder.BuildAsync();
        var options = pipeline.Services
            .GetRequiredService<IOptions<RegistrationOrderOptions>>()
            .Value;

        await Assert.That(options.Calls)
            .IsEquivalentTo(["logging-first", "application-second", "logging-third"],
                TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task LoggingServiceViewRemoveUsesEarliestSharedRegistration()
    {
        var builder = Pipeline.CreateBuilder();
        var duplicate = ServiceDescriptor.Singleton(new object());
        var middle = ServiceDescriptor.Singleton(new Uri("https://example.com"));
        builder.Services.Add(duplicate);
        builder.Logging.Services.Add(middle);
        builder.Logging.Services.Add(duplicate);

        var removed = builder.Logging.Services.Remove(duplicate);
        var remaining = builder.Logging.Services
            .Where(descriptor => ReferenceEquals(descriptor, duplicate)
                                 || ReferenceEquals(descriptor, middle))
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(removed).IsTrue();
            await Assert.That(remaining).Count().IsEqualTo(2);
            await Assert.That(remaining[0]).IsSameReferenceAs(middle);
            await Assert.That(remaining[1]).IsSameReferenceAs(duplicate);
        }
    }

    [Test]
    public async Task LoggingServiceViewReplaceUsesSharedRegistrationOrder()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<OptionsTestModule>();
        builder.Services.Configure<RegistrationOrderOptions>(options =>
            options.Calls.Add("application-first"));
        builder.Logging.Services.Configure<RegistrationOrderOptions>(options =>
            options.Calls.Add("logging-second"));
        builder.Logging.Services.Replace(
            ServiceDescriptor.Singleton<IConfigureOptions<RegistrationOrderOptions>>(
                new ConfigureNamedOptions<RegistrationOrderOptions>(
                    Microsoft.Extensions.Options.Options.DefaultName,
                    options => options.Calls.Add("replacement"))));

        await using var pipeline = await builder.BuildAsync();
        var options = pipeline.Services
            .GetRequiredService<IOptions<RegistrationOrderOptions>>()
            .Value;

        await Assert.That(options.Calls)
            .IsEquivalentTo(["logging-second", "replacement"],
                TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task LoggingInsertAtBoundaryPrecedesApplicationRegistration()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<OptionsTestModule>();
        var loggingBoundary = builder.Logging.Services.Count;
        builder.Services.Configure<RegistrationOrderOptions>(options =>
            options.Calls.Add("application-second"));
        builder.Logging.Services.Insert(
            loggingBoundary,
            ServiceDescriptor.Singleton<IConfigureOptions<RegistrationOrderOptions>>(
                new ConfigureNamedOptions<RegistrationOrderOptions>(
                    Microsoft.Extensions.Options.Options.DefaultName,
                    options => options.Calls.Add("logging-first"))));

        await using var pipeline = await builder.BuildAsync();
        var options = pipeline.Services
            .GetRequiredService<IOptions<RegistrationOrderOptions>>()
            .Value;

        await Assert.That(options.Calls)
            .IsEquivalentTo(["logging-first", "application-second"],
                TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task PipelineBuilderLoggingCanClearFrameworkProviders()
    {
        var loggerProvider = new TestLoggerProvider();
        var builder = TestPipelineBuilder.Create()
            .AddModule<OptionsTestModule>();
        builder.Logging.ClearProviders().AddProvider(loggerProvider);

        await using var pipeline = await builder.BuildAsync();
        var providers = pipeline.Services.GetServices<ILoggerProvider>().ToArray();
        var summary = await pipeline.RunAsync();

        using (Assert.Multiple())
        {
            await Assert.That(providers).Count().IsEqualTo(2);
            await Assert.That(providers.Contains(loggerProvider)).IsTrue();
            await Assert.That(providers.OfType<BuildSystemLogIssueLoggerProvider>()).HasSingleItem();
            await Assert.That(summary.Status).IsEqualTo(ModuleStatus.Succeeded);
        }
    }

    [Test]
    public async Task PipelineBuilderConsoleLoggingReportsEffectiveFilter()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<OptionsTestModule>();
        builder.Logging
            .ClearProviders()
            .AddConsole()
            .SetMinimumLevel(LogLevel.Error);

        await using var pipeline = await builder.BuildAsync();
        var control = pipeline.Services
            .GetRequiredService<MEL.Spectre.ISpectreConsoleLoggerControl>();

        using (Assert.Multiple())
        {
            await Assert.That(control.WouldRender("Category", LogLevel.Information)).IsFalse();
            await Assert.That(control.WouldRender("Category", LogLevel.Error)).IsTrue();
        }
    }

    [Test]
    public async Task TestPipelineBuilderClearsSpectreLoggingProvider()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<OptionsTestModule>();

        await using var pipeline = await builder.BuildAsync();
        var providerTypeNames = pipeline.Services
            .GetServices<ILoggerProvider>()
            .Select(static provider => provider.GetType().FullName)
            .ToArray();

        await Assert.That(providerTypeNames)
            .DoesNotContain("MEL.Spectre.Provider.SpectreConsoleLoggerProvider");
    }

    [Test]
    public async Task PipelineBuilderPreservesRegisteredPipelineOptionsValidators()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<OptionsTestModule>();
        builder.Services
            .AddOptions<PipelineOptions>()
            .Validate(_ => false, "Custom validation failure.")
            .ValidateOnStart();

        var exception = await Assert.ThrowsAsync<OptionsValidationException>(
            () => builder.BuildAsync());

        await Assert.That(exception!.Failures)
            .Contains("Custom validation failure.");
    }

    [Test]
    public async Task PipelineBuilderPreservesNamedPipelineOptionsValidators()
    {
        var configureCalled = false;
        var builder = TestPipelineBuilder.Create()
            .AddModule<OptionsTestModule>();
        builder.Services
            .AddOptions<PipelineOptions>("worker")
            .Configure(_ => configureCalled = true)
            .Validate(_ => false, "Named validation failure.")
            .ValidateOnStart();

        var exception = await Assert.ThrowsAsync<OptionsValidationException>(
            () => builder.BuildAsync());

        using (Assert.Multiple())
        {
            await Assert.That(configureCalled).IsTrue();
            await Assert.That(exception!.OptionsName).IsEqualTo("worker");
            await Assert.That(exception.Failures).Contains("Named validation failure.");
        }
    }

    [Test]
    public async Task SecretMaskingOptionsUseFinalConfiguredPipelineOptions()
    {
        var expected = new SecretMaskingOptions { MaskValue = "[hidden]" };
        var builder = TestPipelineBuilder.Create()
            .AddModule<OptionsTestModule>();
        builder.Services.PostConfigure<PipelineOptions>(options =>
            typeof(PipelineOptions)
                .GetProperty(nameof(PipelineOptions.Secrets))!
                .SetValue(options, expected));

        await using var pipeline = await builder.BuildAsync();
        var pipelineOptions = pipeline.Services
            .GetRequiredService<IOptions<PipelineOptions>>()
            .Value;
        var secretOptions = pipeline.Services
            .GetRequiredService<IOptions<SecretMaskingOptions>>()
            .Value;
        var secretSnapshot = pipeline.Services
            .GetRequiredService<IOptionsSnapshot<SecretMaskingOptions>>()
            .Value;
        var secretMonitor = pipeline.Services
            .GetRequiredService<IOptionsMonitor<SecretMaskingOptions>>()
            .CurrentValue;
        var secretFactory = pipeline.Services
            .GetRequiredService<IOptionsFactory<SecretMaskingOptions>>()
            .Create(Microsoft.Extensions.Options.Options.DefaultName);

        using (Assert.Multiple())
        {
            await Assert.That(pipelineOptions.Secrets).IsSameReferenceAs(expected);
            await Assert.That(secretOptions).IsSameReferenceAs(expected);
            await Assert.That(secretSnapshot).IsSameReferenceAs(expected);
            await Assert.That(secretMonitor).IsSameReferenceAs(expected);
            await Assert.That(secretFactory).IsSameReferenceAs(expected);
        }
    }

    [Test]
    public async Task PipelineBuilderAppliesRegisteredPipelineOptionsConfiguration()
    {
        var configureCalled = false;
        var postConfigureCalledAfterConfigure = false;
        var builder = TestPipelineBuilder.Create()
            .AddModule<OptionsTestModule>();
        builder.Services
            .AddOptions<PipelineOptions>()
            .Configure(_ => configureCalled = true)
            .PostConfigure(_ => postConfigureCalledAfterConfigure = configureCalled)
            .Validate(_ => postConfigureCalledAfterConfigure, "Configuration callbacks were not applied.")
            .ValidateOnStart();

        await using var pipeline = await builder.BuildAsync();

        using (Assert.Multiple())
        {
            await Assert.That(configureCalled).IsTrue();
            await Assert.That(postConfigureCalledAfterConfigure).IsTrue();
        }
    }

    [Test]
    public async Task NamedPipelineOptionsDoNotInheritDefaultConfiguration()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<OptionsTestModule>();
        builder.Services
            .PostConfigure<PipelineOptions>(options =>
                typeof(PipelineOptions)
                    .GetProperty(nameof(PipelineOptions.DisableModuleCache))!
                    .SetValue(options, true))
            .AddOptions<PipelineOptions>("worker");

        await using var pipeline = await builder.BuildAsync();
        var defaultOptions = pipeline.Services
            .GetRequiredService<IOptions<PipelineOptions>>()
            .Value;
        var namedOptions = pipeline.Services
            .GetRequiredService<IOptionsMonitor<PipelineOptions>>()
            .Get("worker");

        using (Assert.Multiple())
        {
            await Assert.That(defaultOptions.DisableModuleCache).IsTrue();
            await Assert.That(namedOptions.DisableModuleCache).IsFalse();
        }
    }

    [Test]
    public async Task PipelineBuilderConsoleLoggingHonorsOverlappingWildcardFilter()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<OptionsTestModule>();
        builder.Logging
            .ClearProviders()
            .AddConsole()
            .AddFilter<Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider>(
                "Abc*Abc",
                LogLevel.None);

        await using var pipeline = await builder.BuildAsync();
        var control = pipeline.Services
            .GetRequiredService<MEL.Spectre.ISpectreConsoleLoggerControl>();

        await Assert.That(control.WouldRender("Abc", LogLevel.Information)).IsFalse();
    }

    [Test]
    public async Task PipelineBuilderConsoleLoggingHonorsCustomLoggerFactoryWithDefaultProviders()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<OptionsTestModule>();
        builder.Services.TryAddSingleton<ILoggerFactory>(NullLoggerFactory.Instance);

        await using var pipeline = await builder.BuildAsync();
        var control = pipeline.Services
            .GetRequiredService<MEL.Spectre.ISpectreConsoleLoggerControl>();

        await Assert.That(control.WouldRender("Category", LogLevel.Error)).IsFalse();
    }

    [Test]
    public async Task NamedPipelineOptionsDoNotShareNestedConfiguration()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<OptionsTestModule>();
        builder.ConfigureOptions(options => options with
        {
            Http = options.Http with
            {
                Logging = new HttpLoggingOptions { LogRequest = true },
            },
        });
        builder.Services
            .AddOptions<PipelineOptions>("worker")
            .Configure(options => typeof(HttpLoggingOptions)
                .GetProperty(nameof(HttpLoggingOptions.LogRequest))!
                .SetValue(options.Http.Logging, false));

        await using var pipeline = await builder.BuildAsync();
        var monitor = pipeline.Services
            .GetRequiredService<IOptionsMonitor<PipelineOptions>>();
        var namedOptions = monitor.Get("worker");
        var defaultOptions = monitor.CurrentValue;

        using (Assert.Multiple())
        {
            await Assert.That(namedOptions.Http.Logging!.LogRequest).IsFalse();
            await Assert.That(defaultOptions.Http.Logging!.LogRequest).IsTrue();
            await Assert.That(namedOptions.Http).IsNotSameReferenceAs(defaultOptions.Http);
            await Assert.That(namedOptions.Http.Logging)
                .IsNotSameReferenceAs(defaultOptions.Http.Logging);
        }
    }

    [Test]
    public async Task NamedPipelineOptionsBindingStartsFromPristineNestedConfiguration()
    {
        const int initialParallelism = 13;
        const int defaultParallelism = 7;
        var builder = TestPipelineBuilder.Create()
            .AddModule<OptionsTestModule>();
        builder.ConfigureOptions(options => options with
        {
            Concurrency = options.Concurrency with
            {
                MaxParallelism = initialParallelism,
            },
        });
        builder.Configuration["DefaultPipeline:Concurrency:MaxParallelism"] =
            defaultParallelism.ToString(System.Globalization.CultureInfo.InvariantCulture);
        builder.Configuration["WorkerPipeline:DisableModuleCache"] = "true";
        builder.Services
            .AddOptions<PipelineOptions>()
            .BindConfiguration("DefaultPipeline");
        builder.Services
            .AddOptions<PipelineOptions>("worker")
            .BindConfiguration("WorkerPipeline");

        await using var pipeline = await builder.BuildAsync();
        var monitor = pipeline.Services.GetRequiredService<IOptionsMonitor<PipelineOptions>>();
        var defaultOptions = monitor.CurrentValue;
        var namedOptions = monitor.Get("worker");

        using (Assert.Multiple())
        {
            await Assert.That(defaultOptions.Concurrency.MaxParallelism)
                .IsEqualTo(defaultParallelism);
            await Assert.That(namedOptions.Concurrency.MaxParallelism)
                .IsEqualTo(initialParallelism);
            await Assert.That(namedOptions.DisableModuleCache).IsTrue();
        }
    }
}
