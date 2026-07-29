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
    public async Task PublicProperties_AreInitOnly()
    {
        var mutableProperties = typeof(PipelineOptions)
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
    public async Task PipelineBuilder_RegistersOptionsSnapshotWithoutCopyingProperties()
    {
        var builder = TestPipelineHostBuilder.Create()
            .AddModule<OptionsTestModule>();
        var expected = builder.Options;

        await using var pipeline = await builder.BuildAsync();
        var actual = pipeline.Services
            .GetRequiredService<IOptions<PipelineOptions>>()
            .Value;

        await Assert.That(actual).IsSameReferenceAs(expected);
    }
}
