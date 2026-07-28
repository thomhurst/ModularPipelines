using ModularPipelines.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.State;

public class PublicStateCompatibilityTests
{
    [Test]
    public async Task RemovedLegacyStateTypesAreNotExposed()
    {
        var assembly = typeof(IModule).Assembly;
        var removedTypeNames = new[]
        {
            "ModularPipelines.Attributes.MandatoryRunConditionAttribute",
            "ModularPipelines.Attributes.RunConditionAttribute",
            "ModularPipelines.Attributes.RunOnLinuxAttribute",
            "ModularPipelines.Attributes.RunOnLinuxOnlyAttribute",
            "ModularPipelines.Attributes.RunOnMacOSAttribute",
            "ModularPipelines.Attributes.RunOnMacOSOnlyAttribute",
            "ModularPipelines.Attributes.RunOnWindowsAttribute",
            "ModularPipelines.Attributes.RunOnWindowsOnlyAttribute",
            "ModularPipelines.Engine.State.ModuleExecutionPhase",
            "ModularPipelines.Engine.State.ModuleStateSnapshot",
            "ModularPipelines.Extensions.PipelineExtensions",
        };

        foreach (var typeName in removedTypeNames)
        {
            await Assert.That(assembly.GetType(typeName)).IsNull();
        }

        await Assert.That(typeof(IPipeline).GetProperty("RootServices")).IsNull();
        await Assert.That(typeof(PipelineBuilderExtensions).GetMethod("BuildHostAsync")).IsNull();
    }
}
