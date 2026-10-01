namespace ModularPipelines.Distributed;

/// <summary>
/// Describes the distributed master as observed by a worker.
/// </summary>
public enum DistributedMasterState
{
    /// <summary>The master is running, or has not started yet.</summary>
    Running,

    /// <summary>The master signalled completion; no further work will be assigned or collected.</summary>
    Completed,

    /// <summary>The master exited, failed or crashed without signalling completion, or cannot be reached.</summary>
    Lost,
}
