namespace ModularPipelines.Distributed;

/// <summary>
/// Thrown when a new worker tries to register after the distributed master has completed.
/// Reconnecting an existing worker session remains permitted.
/// </summary>
public sealed class DistributedRunCompletedException : InvalidOperationException
{
    /// <summary>Initializes a new instance of the <see cref="DistributedRunCompletedException"/> class.</summary>
    public DistributedRunCompletedException()
        : base("The distributed run has completed and no longer accepts new workers.")
    {
    }
}
