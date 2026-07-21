using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.State;

public class PublicStateCompatibilityTests
{
    [Test]
    public async Task LegacyPublicStateTypesRemainAvailableButObsolete()
    {
        var assembly = typeof(IModule).Assembly;
        var typeNames = new[]
        {
            "ModularPipelines.Engine.State.ModuleExecutionPhase",
            "ModularPipelines.Engine.State.ModuleStateSnapshot",
        };

        foreach (var typeName in typeNames)
        {
            var type = assembly.GetType(typeName);

            await Assert.That(type).IsNotNull();
            await Assert.That(type!.IsPublic).IsTrue();
            await Assert.That(type.GetCustomAttributes(typeof(ObsoleteAttribute), false)).IsNotEmpty();
        }
    }
}
