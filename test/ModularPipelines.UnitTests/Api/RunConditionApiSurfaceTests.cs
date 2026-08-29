using ModularPipelines.Attributes;
using ModularPipelines.Conditions;

namespace ModularPipelines.UnitTests.Api;

public class RunConditionApiSurfaceTests
{
    [Test]
    public async Task SingularAndGroupedRunConditionsHaveDistinctContracts()
    {
        var assembly = typeof(RunConditionAttribute).Assembly;

        using (Assert.Multiple())
        {
            await Assert.That(typeof(RunIfAttribute).IsAbstract).IsTrue();
            await Assert.That(typeof(RunIfAttribute<>).IsSealed).IsTrue();
            await Assert.That(assembly.GetType("ModularPipelines.Attributes.RunIfAllAttribute`1"))
                .IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.Attributes.RunIfAnyAttribute`1"))
                .IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.Attributes.RunIfAllAttribute`2"))
                .IsNotNull();
            await Assert.That(assembly.GetType("ModularPipelines.Attributes.RunIfAnyAttribute`2"))
                .IsNotNull();
        }
    }

    [Test]
    public async Task BuiltInConditionsUseSentenceStyleNames()
    {
        var assembly = typeof(OnCI).Assembly;

        using (Assert.Multiple())
        {
            await Assert.That(typeof(OnCI).IsPublic).IsTrue();
            await Assert.That(typeof(OnLocal).IsPublic).IsTrue();
            await Assert.That(typeof(OnFreeBSD).IsPublic).IsTrue();
            await Assert.That(assembly.GetType("ModularPipelines.Conditions.IsCI")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.Conditions.IsLocal")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.OperatingSystemIdentifier")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.OperatingSystemHelper")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.Attributes.RunIfOperatingSystemAttribute"))
                .IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.Attributes.SkipIfOperatingSystemAttribute"))
                .IsNull();
        }
    }
}
