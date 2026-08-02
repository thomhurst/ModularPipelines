using ModularPipelines.Engine;
using ModularPipelines.Models;

namespace ModularPipelines.UnitTests.Engine;

public class DependencySkipDecisionFactoryTests
{
    [Test]
    public async Task Create_CollapsesNestedDependencyReasonsToRootCause()
    {
        var rootDecision = SkipDecision.Skip("Disabled by configuration");
        var intermediateDecision = DependencySkipDecisionFactory.Create(
            [(typeof(RootDependency), rootDecision)]);

        var decision = DependencySkipDecisionFactory.Create(
            [(typeof(IntermediateDependency), intermediateDecision)]);

        await Assert.That(decision.Reason).IsEqualTo(
            "Required dependency 'IntermediateDependency' was skipped: Disabled by configuration");
    }

    [Test]
    [Arguments(null)]
    [Arguments("   ")]
    public async Task Create_CollapsesNestedDependencyReasonsWithoutRootCause(string? rootReason)
    {
        var rootDecision = SkipDecision.Skip(rootReason);
        var intermediateDecision = DependencySkipDecisionFactory.Create(
            [(typeof(RootDependency), rootDecision)]);

        var decision = DependencySkipDecisionFactory.Create(
            [(typeof(IntermediateDependency), intermediateDecision)]);

        await Assert.That(decision.Reason).IsEqualTo(
            "Required dependency 'IntermediateDependency' was skipped");
    }

    private sealed class RootDependency
    {
    }

    private sealed class IntermediateDependency
    {
    }
}
