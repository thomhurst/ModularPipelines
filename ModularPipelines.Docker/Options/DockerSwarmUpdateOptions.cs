using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm update")]
public record DockerSwarmUpdateOptions : DockerOptions
{
    [CommandLongSwitch("autolock")]
    public string? Autolock { get; set; }

    [CommandLongSwitch("cert-expiry")]
    public string? CertExpiry { get; set; }

    [CommandLongSwitch("dispatcher-heartbeat")]
    public string? DispatcherHeartbeat { get; set; }

    [CommandLongSwitch("external-ca")]
    public string? ExternalCa { get; set; }

    [CommandLongSwitch("max-snapshots")]
    public string? MaxSnapshots { get; set; }

    [CommandLongSwitch("snapshot-interval")]
    public string? SnapshotInterval { get; set; }

    [CommandLongSwitch("task-history-limit")]
    public string? TaskHistoryLimit { get; set; }

}