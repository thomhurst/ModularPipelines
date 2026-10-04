namespace ModularPipelines.Distributed.UnitTests;

public class DistributedNamespaceTests
{
    [Test]
    [Arguments(typeof(Capability))]
    [Arguments(typeof(CapabilityRequirement))]
    [Arguments(typeof(ICapabilityProvider))]
    [Arguments(typeof(CapabilityPipelineBuilderExtensions))]
    public async Task Capability_Types_Are_In_The_Root_Namespace(Type type)
    {
        await Assert.That(type.Namespace).IsEqualTo("ModularPipelines");
        await Assert.That(type.Assembly.GetType($"ModularPipelines.Distributed.{type.Name}"))
            .IsNull();
    }

    [Test]
    public async Task Golden_Path_Types_Are_In_The_Distributed_Root_Namespace()
    {
        Type[] types =
        [
            typeof(DistributedPipelineBuilderExtensions),
            typeof(IMasterDiscovery),
        ];

        foreach (var type in types)
        {
            await Assert.That(type.Namespace).IsEqualTo("ModularPipelines.Distributed");
        }
    }

    [Test]
    public async Task Previous_Distributed_Type_Kind_Namespaces_Are_Empty()
    {
        var assembly = typeof(DistributedOptions).Assembly;

        await Assert.That(assembly.GetType(
                "ModularPipelines.Distributed.Extensions.DistributedPipelineBuilderExtensions"))
            .IsNull();
        await Assert.That(assembly.GetType(
                "ModularPipelines.Distributed.Capabilities.CapabilityMatcher"))
            .IsNull();
    }
}
