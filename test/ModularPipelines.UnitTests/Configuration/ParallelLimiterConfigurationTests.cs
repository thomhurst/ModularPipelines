using System.Reflection;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Reporting;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Configuration;

public class ParallelLimiterConfigurationTests
{
    [Test]
    public async Task AttributeUsage_PermitsOnlyClasses()
    {
        foreach (var type in new[] { typeof(ModularPipelines.Attributes.ParallelLimiterAttribute), typeof(ModularPipelines.Attributes.ParallelLimiterAttribute<SingleLimit>) })
        {
            var usage = type.GetCustomAttribute<AttributeUsageAttribute>()!;
            await Assert.That(usage.ValidOn).IsEqualTo(AttributeTargets.Class);
            await Assert.That(usage.Inherited).IsTrue();
            await Assert.That(usage.AllowMultiple).IsFalse();
        }
    }

    [Test]
    public async Task Builder_StoresLastLimiter_WithoutChangingEarlierConfiguration()
    {
        var builder = new ModuleConfigurationBuilder();
        await Assert.That(builder.Build().ParallelLimiterType).IsNull();
        var returnedBuilder = builder.WithParallelLimiter<SingleLimit>();
        var first = builder.Build();
        var second = builder.WithParallelLimiter<OtherLimit>().Build();

        await Assert.That(returnedBuilder).IsSameReferenceAs(builder);
        await Assert.That(first.ParallelLimiterType).IsEqualTo(typeof(SingleLimit));
        await Assert.That(second.ParallelLimiterType).IsEqualTo(typeof(OtherLimit));
    }

    [Test]
    public async Task Configuration_UsesInheritedAttribute_AndFluentOverride()
    {
        IModule inherited = new InheritedAttributeModule();
        IModule configured = new FluentModule();
        IModule attributeOverride = new OtherAttributeModule();
        IModule pureFluent = new PureFluentModule();

        await Assert.That(inherited.Configuration.ParallelLimiterType).IsEqualTo(typeof(SingleLimit));
        await Assert.That(configured.Configuration.ParallelLimiterType).IsEqualTo(typeof(SingleLimit));
        await Assert.That(attributeOverride.Configuration.ParallelLimiterType).IsEqualTo(typeof(OtherLimit));
        await Assert.That(pureFluent.Configuration.ParallelLimiterType).IsEqualTo(typeof(SingleLimit));
    }

    [Test]
    public async Task Execution_SharesSlotsBetweenAttributeAndFluentConfiguration()
    {
        using var provider = new ParallelLimitProvider(Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));
        var handler = new ParallelLimitHandler(provider, NullLogger<ParallelLimitHandler>.Instance);
        using var cancellation = new CancellationTokenSource(TestHostSettings.DefaultTestTimeout);
        using var attributeSlot = await handler.AcquireParallelLimitAsync(new InheritedAttributeModule(), cancellation.Token);
        var fluentWait = handler.AcquireParallelLimitAsync(new FluentModule(), cancellation.Token);
        await Assert.That(fluentWait.IsCompleted).IsFalse();

        // A different limit type has its own semaphore, even when both limits are one.
        using var otherSlot = await handler.AcquireParallelLimitAsync(new OtherModule(), cancellation.Token);
        attributeSlot.Dispose();
        using var fluentSlot = await fluentWait;
        var attributeWait = handler.AcquireParallelLimitAsync(new AttributeModule(), cancellation.Token);
        await Assert.That(attributeWait.IsCompleted).IsFalse();
        fluentSlot.Dispose();
        using var nextAttributeSlot = await attributeWait;
    }

    [Test]
    public async Task Plan_UsesSharedAttributeAndFluentLimiter_AndHonorsOverride()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<AttributeModule>();
        builder.AddModule<InheritedAttributeModule>();
        builder.AddModule<FluentModule>();
        builder.AddModule<PureFluentModule>();
        builder.AddModuleEstimatedTimeProvider<EstimatedTimeProvider>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();

        await Assert.That(plan.EstimatedDuration).IsEqualTo(TimeSpan.FromMinutes(4));
    }

    [Test]
    public async Task Pipeline_ExecutesMixedLimiterConfiguration()
    {
        var result = await TestPipelineBuilder.Create()
            .AddModule<AttributeModule>()
            .AddModule<InheritedAttributeModule>()
            .AddModule<FluentModule>()
            .AddModule<PureFluentModule>()
            .RunAsync();

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task InvalidFluentLimit_IsRejectedByPlanningAndExecution()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<InvalidFluentModule>();
        await using var pipeline = await builder.BuildAsync();
        await Assert.ThrowsAsync<ArgumentException>(() => pipeline.PlanAsync());

        using var provider = new ParallelLimitProvider(Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));
        var handler = new ParallelLimitHandler(provider, NullLogger<ParallelLimitHandler>.Instance);
        await Assert.ThrowsAsync<ArgumentException>(() => handler.AcquireParallelLimitAsync(
            new InvalidFluentModule(), CancellationToken.None));
    }

    private sealed class SingleLimit : IParallelLimit
    {
        public static int Limit => 1;
    }

    private sealed class OtherLimit : IParallelLimit
    {
        public static int Limit => 1;
    }

    private sealed class InvalidLimit : IParallelLimit
    {
        public static int Limit => 0;
    }

    private abstract class TestModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult(GetType().Name);
    }

    [ModularPipelines.Attributes.ParallelLimiter<SingleLimit>]
    private class AttributeModule : TestModule;

    private sealed class InheritedAttributeModule : AttributeModule;

    [ModularPipelines.Attributes.ParallelLimiter<OtherLimit>]
    private sealed class OtherAttributeModule : AttributeModule;

    private sealed class PureFluentModule : TestModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithParallelLimiter<SingleLimit>();
    }

    [ModularPipelines.Attributes.ParallelLimiter<InvalidLimit>]
    private sealed class FluentModule : AttributeModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithParallelLimiter<SingleLimit>();
    }

    private sealed class OtherModule : AttributeModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithParallelLimiter<OtherLimit>();
    }

    private sealed class InvalidFluentModule : AttributeModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithParallelLimiter<InvalidLimit>();
    }

    private sealed class EstimatedTimeProvider : IModuleEstimatedTimeProvider
    {
        public Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType, CancellationToken cancellationToken)
            => Task.FromResult(TimeSpan.FromMinutes(1));

        public Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration, CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
