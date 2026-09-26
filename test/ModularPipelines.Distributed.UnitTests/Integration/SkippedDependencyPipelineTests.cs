using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Distributed.UnitTests.Integration;

public class SkippedDependencyPipelineTests
{
    [Test]
    [Timeout(30_000)]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Skipped_Barrier_Completes_Downstream_Modules(bool includePublishing, CancellationToken cancellationToken)
    {
        var builder = TestPipelineBuilder.Create();
        builder.AddDistributedMode(options =>
        {
            options.TotalInstances = 1;
            options.MaxParallelism = 2;
            options.ModuleResultTimeout = TimeSpan.FromSeconds(10);
        });
        builder.AddModule<PrevalidatedTests>();
        builder.AddModule<TestBarrier>();
        builder.AddModule<Package>();
        if (includePublishing)
        {
            builder.AddModule<Publish>();
        }

        await using var pipeline = await builder.BuildAsync();
        await pipeline.RunAsync(cancellationToken);

        var results = pipeline.Services.GetRequiredService<IModuleResultRegistry>();
        await Assert.That(results.GetResult(typeof(TestBarrier))!.Status).IsEqualTo(ModuleStatus.Skipped);
        await Assert.That(results.GetResult(typeof(Package))!.Status).IsEqualTo(ModuleStatus.Skipped);
        if (includePublishing)
        {
            await Assert.That(results.GetResult(typeof(Publish))!.Status).IsEqualTo(ModuleStatus.Skipped);
        }
    }

    private sealed class PrevalidatedTests : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => SkipDecision.Skip("Validated by the prerequisite job"));

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("The prevalidated test command must not run.");
    }

    [DependsOn<PrevalidatedTests>]
    private sealed class TestBarrier : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("The skipped barrier must not run.");
    }

    [DependsOn<TestBarrier>]
    private sealed class Package : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("The package must not run after a skipped barrier.");
    }

    [DependsOn<Package>]
    private sealed class Publish : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => SkipDecision.Skip("Publishing is disabled"));

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Publishing is disabled.");
    }
}
