using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm", "init")]
[ExcludeFromCodeCoverage]
public record DockerSwarmInitOptions : DockerOptions
{
    [CommandSwitch("--advertise-addr")]
    public virtual string? AdvertiseAddr { get; set; }

    [CommandSwitch("--autolock")]
    public virtual string? Autolock { get; set; }

    [CommandSwitch("--availability")]
    public virtual string? Availability { get; set; }

    [CommandSwitch("--cert-expiry")]
    public virtual string? CertExpiry { get; set; }

    [CommandSwitch("--data-path-addr")]
    public virtual string? DataPathAddr { get; set; }

    [CommandSwitch("--data-path-port")]
    public virtual string? DataPathPort { get; set; }

    [CommandSwitch("--default-addr-pool")]
    public virtual string? DefaultAddrPool { get; set; }

    [CommandSwitch("--default-addr-pool-mask-length")]
    public virtual int? DefaultAddrPoolMaskLength { get; set; }

    [CommandSwitch("--dispatcher-heartbeat")]
    public virtual string? DispatcherHeartbeat { get; set; }

    [CommandSwitch("--external-ca")]
    public virtual string? ExternalCa { get; set; }

    [CommandSwitch("--force-new-cluster")]
    public virtual string? ForceNewCluster { get; set; }

    [CommandSwitch("--listen-addr")]
    public virtual string? ListenAddr { get; set; }

    [CommandSwitch("--max-snapshots")]
    public virtual string? MaxSnapshots { get; set; }

    [CommandSwitch("--snapshot-interval")]
    public virtual int? SnapshotInterval { get; set; }

    [CommandSwitch("--task-history-limit")]
    public virtual int? TaskHistoryLimit { get; set; }
}
