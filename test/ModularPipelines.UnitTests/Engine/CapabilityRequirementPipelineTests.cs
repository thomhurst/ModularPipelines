using ModularPipelines.Attributes;
using ModularPipelines.Distributed;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Engine;

public class CapabilityRequirementPipelineTests
{
    [Test]
    public async Task Local_Pipeline_Skips_Module_Missing_Declared_Capability()
    {
        var summary = await TestPipelineBuilder.Create()
            .AddModule<GpuModule>()
            .AddModule<CurrentOperatingSystemModule>()
            .RunAsync();

        var gpuResult = await summary.Modules.OfType<GpuModule>().Single();
        var operatingSystemResult = await summary.Modules.OfType<CurrentOperatingSystemModule>().Single();

        using (Assert.Multiple())
        {
            await Assert.That(gpuResult.Status).IsEqualTo(ModuleStatus.Skipped);
            await Assert.That(operatingSystemResult.Status).IsEqualTo(ModuleStatus.Succeeded);
        }
    }

    [Test]
    public async Task Local_Pipeline_Runs_Module_With_Added_Capability()
    {
        var builder = TestPipelineBuilder.Create();
        builder.AddCapabilities(Capability.Gpu);

        var summary = await builder
            .AddModule<GpuModule>()
            .RunAsync();

        var result = await summary.Modules.OfType<GpuModule>().Single();

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Local_Pipeline_Runs_Module_With_Provided_Capability()
    {
        var builder = TestPipelineBuilder.Create();
        builder.AddCapabilityProvider<GpuProvider>();

        var summary = await builder
            .AddModule<GpuModule>()
            .RunAsync();

        var result = await summary.Modules.OfType<GpuModule>().Single();

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [RequiresCapability(Capability.Names.Gpu)]
    private sealed class GpuModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RequiresAnyCapability(
        Capability.Names.Windows,
        Capability.Names.Linux,
        Capability.Names.MacOS,
        Capability.Names.FreeBSD)]
    private sealed class CurrentOperatingSystemModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private sealed class GpuProvider : ICapabilityProvider
    {
        public Task<IEnumerable<Capability>> GetCapabilitiesAsync(CancellationToken cancellationToken) =>
            Task.FromResult<IEnumerable<Capability>>([Capability.Gpu]);
    }
}
