namespace ModularPipelines.Distributed.SignalR.Hub;

/// <summary>
/// Hub method names shared by the master hub and the worker connection. Every call is a
/// worker-to-master invocation; the master never pushes to workers, so a worker that reconnects
/// resumes by invoking again.
/// </summary>
internal static class HubMethodNames
{
    public const string RegisterWorker = "RegisterWorker";
    public const string Heartbeat = "Heartbeat";
    public const string DequeueModule = "DequeueModule";
    public const string PublishResult = "PublishResult";
    public const string WaitForResult = "WaitForResult";
    public const string WaitForCancellation = "WaitForCancellation";
}
