namespace ModularPipelines.UnitTests.Api;

public class RunConditionApiSurfaceTests
{
    [Test]
    public async Task SingularAndGroupedRunConditionsHaveDistinctContracts()
    {
        var assembly = typeof(RunConditionAttribute).Assembly;

        using (Assert.Multiple())
        {
            await Assert.That(typeof(RunConditionAttribute).IsAbstract).IsTrue();
            await Assert.That(typeof(IRunCondition).IsAssignableFrom(typeof(RunConditionAttribute))).IsTrue();
            await Assert.That(typeof(RunIfAttribute<>).IsSealed).IsTrue();
            await Assert.That(typeof(RunIfAttribute<,>).IsSealed).IsTrue();
            await Assert.That(typeof(RunIfAttribute<,,,>).IsSealed).IsTrue();
            await Assert.That(assembly.GetType("ModularPipelines.RunIfAttribute")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.SkipIfAttribute")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.RunIfAnyAttribute")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.RunIfAllAttribute")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.RunIfAllAttribute`2")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.RunIfAnyAttribute`1")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.RunIfAnyAttribute`2")).IsNotNull();
            await Assert.That(assembly.GetType("ModularPipelines.IConditionAttribute")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.IGroupedConditionAttribute")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.IPlanningRunCondition")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.IPlanningSafeDependencySelector")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.PlanningSafeDependsOnBaseAttribute")).IsNull();
            await Assert.That(Enum.GetNames<ConditionLogic>()).IsEquivalentTo(["All", "Any"]);
            await Assert.That(Enum.GetNames<ConditionIntent>()).IsEquivalentTo(["Run", "Skip"]);

            var evaluateMethods = typeof(IRunCondition).GetMethods();
            await Assert.That(evaluateMethods.Length).IsEqualTo(1);
            await Assert.That(evaluateMethods[0].GetParameters().Last().ParameterType)
                .IsEqualTo(typeof(CancellationToken));
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
