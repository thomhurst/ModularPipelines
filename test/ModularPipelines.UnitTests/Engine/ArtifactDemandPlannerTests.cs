using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Models;
using ModularPipelines.Modules;

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

    [Test]
    public async Task DemandPlanCache_Coalesces_Concurrent_Resolution()
    {
        var cache = new ArtifactDemandPlanCache();
        var resolutionStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseResolution = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var resolutionCount = 0;

        async Task<ArtifactDemandPlan> Resolve()
        {
            Interlocked.Increment(ref resolutionCount);
            resolutionStarted.TrySetResult();
            await releaseResolution.Task;
            return CreatePlan(typeof(BEffectiveProducer));
        }

        var first = cache.GetAsync(() => [], Resolve, CancellationToken.None);
        await resolutionStarted.Task;
        var second = cache.GetAsync(() => [], Resolve, CancellationToken.None);
        releaseResolution.SetResult();

        var plans = await Task.WhenAll(first, second);

        using (Assert.Multiple())
        {
            await Assert.That(resolutionCount).IsEqualTo(1);
            await Assert.That(plans[0].RequiredProducerTypes)
                .IsEquivalentTo([typeof(BEffectiveProducer)]);
            await Assert.That(plans[1].RequiredProducerTypes)
                .IsEquivalentTo([typeof(BEffectiveProducer)]);
        }
    }

    [Test]
    public async Task DemandPlanCache_Recomputes_After_Completion_Changes()
    {
        var cache = new ArtifactDemandPlanCache();
        var completed = false;
        var resolutionCount = 0;

        ArtifactDemandPlan Resolve()
        {
            resolutionCount++;
            return CreatePlan(completed ? typeof(CEffectiveProducer) : typeof(BEffectiveProducer));
        }

        var first = await cache.GetAsync(
            () => completed ? [typeof(AUnnecessaryProducer)] : [],
            () => Task.FromResult(Resolve()),
            CancellationToken.None);
        var cached = await cache.GetAsync(
            () => completed ? [typeof(AUnnecessaryProducer)] : [],
            () => Task.FromResult(Resolve()),
            CancellationToken.None);
        completed = true;
        var refreshed = await cache.GetAsync(
            () => completed ? [typeof(AUnnecessaryProducer)] : [],
            () => Task.FromResult(Resolve()),
            CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(resolutionCount).IsEqualTo(2);
            await Assert.That(first.RequiredProducerTypes)
                .IsEquivalentTo([typeof(BEffectiveProducer)]);
            await Assert.That(cached.RequiredProducerTypes)
                .IsEquivalentTo([typeof(BEffectiveProducer)]);
            await Assert.That(refreshed.RequiredProducerTypes)
                .IsEquivalentTo([typeof(CEffectiveProducer)]);
        }
    }

    [Test]
    public async Task DemandPlanCache_Does_Not_Publish_A_Plan_From_A_Changing_Snapshot()
    {
        var cache = new ArtifactDemandPlanCache();
        var completed = false;
        var resolutionCount = 0;

        Task<ArtifactDemandPlan> Resolve()
        {
            resolutionCount++;
            if (resolutionCount == 1)
            {
                completed = true;
            }

            return Task.FromResult(CreatePlan(
                completed ? typeof(CEffectiveProducer) : typeof(BEffectiveProducer)));
        }

        var result = await cache.GetAsync(
            () => completed ? [typeof(AUnnecessaryProducer)] : [],
            Resolve,
            CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(resolutionCount).IsEqualTo(2);
            await Assert.That(result.RequiredProducerTypes)
                .IsEquivalentTo([typeof(CEffectiveProducer)]);
        }
    }

    [Test]
    public async Task ModuleState_Records_Exactly_One_Concurrent_Skip_Decision()
    {
        var module = new TestModule();
        var state = new ModuleState(module, module.GetType());
        var decisions = Enumerable.Range(0, 32)
            .Select(index => SkipDecision.Skip(index.ToString()))
            .ToArray();
        var attempts = await Task.WhenAll(decisions.Select(decision => Task.Run(() =>
            (Decision: decision, Recorded: state.TrySetSkipResult(decision)))));
        var winner = attempts.Single(attempt => attempt.Recorded);

        using (Assert.Multiple())
        {
            await Assert.That(attempts.Count(attempt => attempt.Recorded)).IsEqualTo(1);
            await Assert.That(state.SkipResult).IsSameReferenceAs(winner.Decision);
        }
    }

    private static ArtifactDemandPlan CreatePlan(Type producerType) =>
        new(
            new HashSet<Type> { producerType },
            new Dictionary<Type, IReadOnlySet<string>>());

    private sealed class TestModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult(true);
    }
}
