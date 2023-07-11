using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm update")]
public record DockerSwarmUpdateOptions : DockerOptions
{

    [CommandSwitch("--autolock")]
    public string? Autolock { get; set; }

    [CommandSwitch("--cert-expiry")]
    public string? CertExpiry { get; set; }

    [CommandSwitch("--dispatcher-heartbeat")]
    public string? DispatcherHeartbeat { get; set; }

    [CommandSwitch("--external-ca")]
    public string? ExternalCa { get; set; }

    [CommandSwitch("--max-snapshots")]
    public string? MaxSnapshots { get; set; }

    [CommandSwitch("--snapshot-interval")]
    public int? SnapshotInterval { get; set; }

    [CommandSwitch("--task-history-limit")]
    public int? TaskHistoryLimit { get; set; }

}