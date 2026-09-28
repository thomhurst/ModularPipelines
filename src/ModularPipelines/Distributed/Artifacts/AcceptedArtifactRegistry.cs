using System.Collections.Concurrent;

namespace ModularPipelines.Distributed.Artifacts;

/// <summary>
/// Records the artifacts listed by each module's accepted (first, final) distributed result.
/// </summary>
/// <remarks>
/// Consumers download the references a producer's accepted result lists instead of choosing the
/// newest upload in the store, whose timestamps come from different machines' clocks and can
/// include uploads from a superseded execution.
/// </remarks>
internal sealed class AcceptedArtifactRegistry
{
    private readonly ConcurrentDictionary<ModuleId, IReadOnlyList<ArtifactReference>> _artifacts = new();

    public void Record(ModuleId producer, IReadOnlyList<ArtifactReference> artifacts) =>
        _artifacts.TryAdd(producer, artifacts);

    public void Record(SerializedModuleResult result) => Record(result.ModuleId, result.Artifacts);

    public void RecordConsumed(IReadOnlyList<ArtifactReference> consumedArtifacts)
    {
        foreach (var producer in consumedArtifacts.GroupBy(static artifact => artifact.ModuleId))
        {
            Record(producer.Key, [.. producer]);
        }
    }

    public bool TryGet(ModuleId producer, out IReadOnlyList<ArtifactReference> artifacts)
    {
        if (_artifacts.TryGetValue(producer, out var recorded))
        {
            artifacts = recorded;
            return true;
        }

        artifacts = [];
        return false;
    }
}
