namespace ModularPipelines.Engine;

internal sealed class ArtifactDemandPlanCache
{
    private readonly SemaphoreSlim _gate = new(1, 1);
    private HashSet<Type>? _completedModuleTypes;
    private ArtifactDemandPlan? _plan;

    public async Task<ArtifactDemandPlan> GetAsync(
        Func<HashSet<Type>> getCompletedModuleTypes,
        Func<Task<ArtifactDemandPlan>> resolve,
        CancellationToken cancellationToken)
    {
        await _gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            while (true)
            {
                var completedBeforeResolution = getCompletedModuleTypes();
                if (_plan is not null
                    && _completedModuleTypes!.SetEquals(completedBeforeResolution))
                {
                    return _plan;
                }

                var plan = await resolve().ConfigureAwait(false);
                var completedAfterResolution = getCompletedModuleTypes();
                if (!completedBeforeResolution.SetEquals(completedAfterResolution))
                {
                    continue;
                }

                _completedModuleTypes = completedAfterResolution;
                _plan = plan;
                return plan;
            }
        }
        finally
        {
            _gate.Release();
        }
    }
}

internal sealed record ArtifactDemandPlan(
    IReadOnlySet<Type> RequiredProducerTypes,
    IReadOnlyDictionary<Type, IReadOnlySet<string>> RequiredArtifactsByProducer);
