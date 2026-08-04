using ModularPipelines.Engine;

namespace ModularPipelines.UnitTests.Engine;

public class ArtifactDemandPlannerTests
{
    private sealed class AUnnecessaryProducer;

    private sealed class BEffectiveProducer;

    private sealed class CEffectiveProducer;

    [Test]
    public async Task ResolveAsync_Removes_Ineffective_Cycle_Breakers()
    {
        var allProducers = new HashSet<Type>
        {
            typeof(AUnnecessaryProducer),
            typeof(BEffectiveProducer),
            typeof(CEffectiveProducer),
        };

        var result = await ArtifactDemandPlanner.ResolveAsync(currentDemand =>
        {
            var nextDemand = currentDemand.Contains(typeof(BEffectiveProducer))
                             || currentDemand.Contains(typeof(CEffectiveProducer))
                ? []
                : new HashSet<Type>(allProducers);
            return Task.FromResult(nextDemand);
        });

        await Assert.That(result).IsEquivalentTo([typeof(BEffectiveProducer)]);
    }
}
