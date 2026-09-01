namespace ModularPipelines.Distributed.SignalR.Hub;

/// <summary>
/// String constants for SignalR hub method names.
/// Used by both server (hub) and client (HubConnection) to ensure consistency.
/// </summary>
internal static class HubMethodNames
{
    // Worker -> Master (hub methods)
    public const string RegisterWorker = "RegisterWorker";
    public const string Heartbeat = "Heartbeat";
    public const string PublishResult = "PublishResult";
    public const string RequestWork = "RequestWork";

    // Master -> Worker (client methods)
    public const string ReceiveAssignment = "ReceiveAssignment";
    public const string ReceiveDependencyResult = "ReceiveDependencyResult";
    public const string SignalCompletion = "SignalCompletion";
    public const string BroadcastCancellation = "BroadcastCancellation";
}
