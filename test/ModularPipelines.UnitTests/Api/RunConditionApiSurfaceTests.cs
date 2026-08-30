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
            await Assert.That(assembly.GetType("ModularPipelines.RunIfAllAttribute`1"))
                .IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.RunIfAnyAttribute`1"))
                .IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.RunIfAllAttribute`2"))
                .IsNotNull();
            await Assert.That(assembly.GetType("ModularPipelines.RunIfAnyAttribute`2"))
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
            await Assert.That(assembly.GetType("ModularPipelines.IsCI")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.IsLocal")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.OperatingSystemIdentifier")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.OperatingSystemHelper")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.RunIfOperatingSystemAttribute"))
                .IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.SkipIfOperatingSystemAttribute"))
                .IsNull();
        }
    }
}
