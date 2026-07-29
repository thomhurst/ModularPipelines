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
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(null);
    }

    [Test]
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
    public async Task PipelineBuilder_RegistersConsistentOptionsSnapshotWithoutCopyingProperties()
    {
        var builder = TestPipelineHostBuilder.Create()
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

        await Assert.That(options).IsSameReferenceAs(expected);
        await Assert.That(snapshot).IsSameReferenceAs(expected);
        await Assert.That(monitor).IsSameReferenceAs(expected);
    }
}
