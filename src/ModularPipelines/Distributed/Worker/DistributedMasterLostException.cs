namespace ModularPipelines.Distributed.Worker;

/// <summary>
/// Thrown on a worker when the master stopped without signalling completion.
/// </summary>
internal sealed class DistributedMasterLostException(WorkerId workerId)
    : InvalidOperationException(
        $"The distributed master stopped without signalling completion, so worker '{workerId}' canceled its work. "
        + "Check the master's logs for why it exited, failed or crashed.");
