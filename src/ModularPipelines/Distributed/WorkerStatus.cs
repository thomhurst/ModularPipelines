namespace ModularPipelines.Distributed;

public enum WorkerStatus
{
    Connected,
    Active,
    Executing,
    Disconnected,
    TimedOut
}
