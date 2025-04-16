using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm", "update")]
[ExcludeFromCodeCoverage]
public record DockerSwarmUpdateOptions : DockerOptions
{
    [CommandSwitch("--autolock")]
    public virtual string? Autolock { get; set; }

    [CommandSwitch("--cert-expiry")]
    public virtual string? CertExpiry { get; set; }

    [CommandSwitch("--dispatcher-heartbeat")]
    public virtual string? DispatcherHeartbeat { get; set; }

    [CommandSwitch("--external-ca")]
    public virtual string? ExternalCa { get; set; }

    [CommandSwitch("--max-snapshots")]
    public virtual string? MaxSnapshots { get; set; }

    [CommandSwitch("--snapshot-interval")]
    public virtual int? SnapshotInterval { get; set; }

    [CommandSwitch("--task-history-limit")]
    public virtual int? TaskHistoryLimit { get; set; }
}
