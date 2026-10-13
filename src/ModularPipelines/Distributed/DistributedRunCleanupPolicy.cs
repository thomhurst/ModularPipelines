namespace ModularPipelines.Distributed;

/// <summary>
/// Selects which pipeline outcomes make <see cref="IDistributedRunCleanup"/> backends delete a run's data.
/// </summary>
public enum DistributedRunCleanupPolicy
{
    /// <summary>Deletes the run's data whatever the pipeline outcome.</summary>
    Always,

    /// <summary>
    /// Deletes the run's data only when the pipeline succeeded, so a failed run's data stays available for
    /// investigation until each backend's own expiry removes it.
    /// </summary>
    OnSuccess,

    /// <summary>Never deletes the run's data; each backend's own expiry, if any, removes it.</summary>
    Never,
}
