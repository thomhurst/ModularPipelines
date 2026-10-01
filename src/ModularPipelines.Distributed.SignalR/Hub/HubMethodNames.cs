namespace ModularPipelines.Distributed.SignalR.Hub;

/// <summary>
/// Hub method names shared by the master hub and the worker connection. Every call except
/// <see cref="MasterCompleted"/> is a worker-to-master invocation, so a worker that reconnects
/// resumes by invoking again. The master pushes <see cref="MasterCompleted"/> to every worker
/// when it completes, and again to a worker that registers afterwards.
/// </summary>
internal static class HubMethodNames
{
    public const string RegisterWorker = "RegisterWorker";
    public const string Heartbeat = "Heartbeat";
    public const string DequeueModule = "DequeueModule";
    public const string PublishResult = "PublishResult";
    public const string WaitForResult = "WaitForResult";
    public const string WaitForCancellation = "WaitForCancellation";
    public const string AcknowledgeCompletion = "AcknowledgeCompletion";

    /// <summary>The master-to-worker message announcing that the master has completed.</summary>
    public const string MasterCompleted = "MasterCompleted";
}
