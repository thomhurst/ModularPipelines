namespace ModularPipelines.Distributed;

public record SerializedModuleResult(
    string ModuleTypeName,
    string ResultTypeName,
    int WorkerIndex,
    string SerializedJson,
    DateTimeOffset CompletedAt,
    IReadOnlyList<ArtifactReference>? Artifacts = null);
