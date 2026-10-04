using ModularPipelines.Distributed;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Engine;

public class CapabilityConditionEvaluationTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task DefaultEvaluationUsesDeclaredCapabilities(bool hasCapability)
    {
        var builder = TestPipelineBuilder.Create().AddModule<GpuModule>();
        if (hasCapability)
        {
            builder.AddCapabilities(Capability.Gpu);
        }

        var summary = await builder.RunAsync();
        var result = await summary.Modules.OfType<GpuModule>().Single();

        await Assert.That(result.Status).IsEqualTo(hasCapability ? ModuleStatus.Succeeded : ModuleStatus.Skipped);
    }

    [Test]
    public async Task DefaultEvaluationUsesCapabilityProviders()
    {
        var summary = await TestPipelineBuilder.Create()
            .AddCapabilityProvider<GpuProvider>()
            .AddModule<GpuModule>()
            .RunAsync();
        var result = await summary.Modules.OfType<GpuModule>().Single();

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task RuntimeEvaluationHonorsAnExplicitImplementation()
    {
        var summary = await TestPipelineBuilder.Create()
            .AddCapabilities(Capability.Gpu)
            .AddModule<RejectedGpuModule>()
            .RunAsync();
        var result = await summary.Modules.OfType<RejectedGpuModule>().Single();

        // A matching routing capability must not replace the execution-time predicate.
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Skipped);
    }

    [Test]
    public async Task RoutingUsesCapabilityWithoutCallingRuntimeEvaluation()
    {
        var formula = ConditionFormula.ForAttribute(new RunIfAttribute<RejectGpu>())!;
        var route = CapabilityConditions.GetRoute(new RunIfAttribute<RejectGpu>());

        await Assert.That(formula.Atoms).IsEmpty();
        await Assert.That(formula.Evaluate(_ => throw new InvalidOperationException()).Requirement)
            .IsEqualTo(CapabilityRequirement.AllOf(Capability.Gpu));
        await Assert.That(route!.Requirement).IsEqualTo(CapabilityRequirement.AllOf(Capability.Gpu));
    }

    [Test]
    public async Task DefaultEvaluationHonorsCancellationBeforeResolvingCapabilities()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        IRunCondition condition = new OnGpu();

        await Assert.That(() => condition.EvaluateAsync(Moq.Mock.Of<IPipelineContext>(), cancellation.Token))
            .Throws<OperationCanceledException>();
    }

    private sealed class OnGpu : ICapabilityCondition
    {
        public Capability Capability => Capability.Gpu;
    }

    private sealed class RejectGpu : ICapabilityCondition
    {
        public Capability Capability => Capability.Gpu;

        public Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
            Task.FromResult(false);
    }

    [RunIf<OnGpu>]
    private sealed class GpuModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RunIf<RejectGpu>]
    private sealed class RejectedGpuModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private sealed class GpuProvider : ICapabilityProvider
    {
        public Task<IEnumerable<Capability>> GetCapabilitiesAsync(CancellationToken cancellationToken) =>
            Task.FromResult<IEnumerable<Capability>>([Capability.Gpu]);
    }
}
