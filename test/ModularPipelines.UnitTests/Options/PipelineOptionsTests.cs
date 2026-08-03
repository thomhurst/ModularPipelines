using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using Spectre.Console;

namespace ModularPipelines.UnitTests.Options;

[NotInParallel]
public class PipelineOptionsTests
{
    private sealed class OptionsTestModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    [Test]
    [Arguments(typeof(PipelineBuilderOptions))]
    [Arguments(typeof(PipelineOptions))]
    [Arguments(typeof(ConcurrencyOptions))]
    [Arguments(typeof(HttpLoggingOptions))]
    [Arguments(typeof(HttpResilienceOptions))]
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
    public async Task PipelineBuilderOptions_IsASealedRecord()
    {
        var original = new PipelineBuilderOptions { ApplicationName = "original" };
        var updated = original with { ApplicationName = "updated" };

        using (Assert.Multiple())
        {
            await Assert.That(typeof(PipelineBuilderOptions).IsSealed).IsTrue();
            await Assert.That(original.ApplicationName).IsEqualTo("original");
            await Assert.That(updated.ApplicationName).IsEqualTo("updated");
        }
    }

    [Test]
    public async Task CategoryBuilderMethodsReplaceEarlierFilters()
    {
        using var builder = Pipeline.CreateBuilder();

        builder.RunOnlyCategories("first");
        builder.RunOnlyCategories("second");
        builder.IgnoreCategories("ignored-first");
        builder.IgnoreCategories("ignored-second");

        using (Assert.Multiple())
        {
            await Assert.That(builder.Options.RunOnlyCategories).IsEquivalentTo(["second"]);
            await Assert.That(builder.Options.IgnoreCategories).IsEquivalentTo(["ignored-second"]);
            await Assert.That(typeof(PipelineBuilder).GetMethod("RunCategories")).IsNull();
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
                ShowProgressInConsole = !originalInteractive,
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
        await Assert.That(new PipelineOptions().ShowProgressInConsole)
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
            DefaultHttpLoggingOptions = new HttpLoggingOptions
            {
                SensitiveHeaderNames = sensitiveHeaders,
            },
            DefaultHttpResilienceOptions = new HttpResilienceOptions
            {
                RetryableStatusCodes = retryableStatusCodes,
            },
        };

        sensitiveHeaders.Clear();
        retryableStatusCodes.Add(HttpStatusCode.BadGateway);

        using (Assert.Multiple())
        {
            await Assert.That(options.DefaultHttpLoggingOptions!.SensitiveHeaderNames)
                .IsEquivalentTo(["X-Secret"]);
            await Assert.That(options.DefaultHttpResilienceOptions!.RetryableStatusCodes)
                .IsEquivalentTo([HttpStatusCode.ServiceUnavailable]);
            await Assert.That(
                    ((ICollection<string>) options.DefaultHttpLoggingOptions.SensitiveHeaderNames)
                    .IsReadOnly)
                .IsTrue();
            await Assert.That(
                    ((ICollection<HttpStatusCode>)
                        options.DefaultHttpResilienceOptions.RetryableStatusCodes)
                    .IsReadOnly)
                .IsTrue();
        }
    }

    [Test]
    public async Task DefaultExecutionEnvironmentVariables_AreDefensivelyCopied()
    {
        var environmentVariables = new Dictionary<string, string?>
        {
            ["ORIGINAL"] = "value",
        };
        var options = new PipelineOptions
        {
            DefaultExecutionOptions = new CommandExecutionOptions
            {
                EnvironmentVariables = environmentVariables,
            },
        };

        environmentVariables["ORIGINAL"] = "changed";
        environmentVariables["ADDED"] = "later";

        var snapshot = options.DefaultExecutionOptions!.EnvironmentVariables!;
        using (Assert.Multiple())
        {
            await Assert.That(snapshot["ORIGINAL"]).IsEqualTo("value");
            await Assert.That(snapshot.ContainsKey("ADDED")).IsFalse();
            await Assert.That(
                    ((ICollection<KeyValuePair<string, string?>>) snapshot).IsReadOnly)
                .IsTrue();
        }
    }

    [Test]
    public async Task PipelineBuilder_RegistersEquivalentIsolatedOptionsSnapshots()
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
            await Assert.That(snapshot).IsEqualTo(expected);
            await Assert.That(monitor).IsEqualTo(expected);
            await Assert.That(options).IsNotSameReferenceAs(expected);
            await Assert.That(snapshot).IsNotSameReferenceAs(expected);
            await Assert.That(monitor).IsNotSameReferenceAs(expected);
        }
    }

    [Test]
    public async Task PipelineOptionsFactory_IsolatesNamedConfigurations()
    {
        var source = new PipelineOptions();
        var namedSetup = new ConfigureNamedOptions<PipelineOptions>(
            "custom",
            options => typeof(PipelineOptions)
                .GetProperty(nameof(PipelineOptions.PrintLogo))!
                .SetValue(options, false));
        var factory = new PipelineOptionsFactory(
            source,
            [namedSetup],
            [],
            []);

        var custom = factory.Create("custom");
        var defaults = factory.Create(Microsoft.Extensions.Options.Options.DefaultName);

        using (Assert.Multiple())
        {
            await Assert.That(custom.PrintLogo).IsFalse();
            await Assert.That(defaults.PrintLogo).IsTrue();
            await Assert.That(source.PrintLogo).IsTrue();
            await Assert.That(custom).IsNotSameReferenceAs(defaults);
            await Assert.That(custom).IsNotSameReferenceAs(source);
            await Assert.That(defaults).IsNotSameReferenceAs(source);
        }
    }

    [Test]
    public async Task PipelineBuilder_PreservesRegisteredPipelineOptionsValidators()
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
