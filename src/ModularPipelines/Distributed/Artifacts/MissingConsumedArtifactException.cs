namespace ModularPipelines.Distributed.Artifacts;

internal sealed class MissingConsumedArtifactException(
    Type producerModuleType,
    string artifactName,
    Type consumerModuleType)
    : InvalidOperationException(
        $"Artifact '{artifactName}' from module '{producerModuleType.FullName}' "
        + $"was not found for consumer '{consumerModuleType.Name}'.")
{
    public Type ProducerModuleType { get; } = producerModuleType;

    public string ArtifactName { get; } = artifactName;

    public Type ConsumerModuleType { get; } = consumerModuleType;
}
