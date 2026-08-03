namespace ModularPipelines.GitHub.PipelineWriters;

/// <summary>
/// The coordination backend used by a generated distributed workflow.
/// </summary>
public enum DistributedBackend
{
    /// <summary>
    /// Use a Redis connection supplied through a GitHub Actions secret.
    /// </summary>
    Redis,
}
