namespace ModularPipelines.Distributed;

internal sealed class DistributedModuleExecutionTimer(
    DateTimeOffset claimedAt,
    TimeSpan dependencyResultProcessingDuration)
{
    private DateTimeOffset? _executionStartedAt;
    private DateTimeOffset? _executionFinishedAt;

    public TimeSpan ArtifactDownloadDuration { get; set; }

    public TimeSpan ArtifactUploadDuration { get; set; }

    public void StartExecution() => _executionStartedAt = DateTimeOffset.UtcNow;

    public void FinishExecution() => _executionFinishedAt = DateTimeOffset.UtcNow;

    public DistributedModuleExecutionTelemetry CreateTelemetry()
    {
        var now = DateTimeOffset.UtcNow;
        var executionStartedAt = _executionStartedAt ?? now;
        var executionFinishedAt = _executionFinishedAt ?? now;
        if (executionFinishedAt < executionStartedAt)
        {
            executionFinishedAt = executionStartedAt;
        }

        return new DistributedModuleExecutionTelemetry
        {
            ClaimedAt = claimedAt,
            ExecutionStartedAt = executionStartedAt,
            ExecutionFinishedAt = executionFinishedAt,
            DependencyResultProcessingDuration = dependencyResultProcessingDuration,
            ArtifactDownloadDuration = ArtifactDownloadDuration,
            ArtifactUploadDuration = ArtifactUploadDuration,
        };
    }
}
