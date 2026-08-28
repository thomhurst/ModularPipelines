using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using ModularPipelines.Options;
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
            await Assert.That(options).IsSameReferenceAs(expected);
            await Assert.That(snapshot).IsSameReferenceAs(expected);
            await Assert.That(monitor).IsSameReferenceAs(expected);
        }
    }

    [Test]
    public async Task ConfigureOptionsRejectsNullResult()
    {
        using var builder = Pipeline.CreateBuilder();

        var exception = Assert.Throws<InvalidOperationException>(
            () => builder.ConfigureOptions(_ => null!));

        await Assert.That(exception.Message)
            .IsEqualTo("The pipeline options configuration returned null.");
    }

    [Test]
    public async Task PipelineBuilderExposesLoggingBuilder()
    {
        using var builder = Pipeline.CreateBuilder();

        await Assert.That(builder.Logging.Services).IsSameReferenceAs(builder.Services);
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
            await Assert.That(providers).HasSingleItem();
            await Assert.That(providers[0]).IsSameReferenceAs(loggerProvider);
            await Assert.That(summary.Status).IsEqualTo(ModuleStatus.Succeeded);
        }
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
}
