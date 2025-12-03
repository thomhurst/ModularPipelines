using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("swarm", "init")]
[ExcludeFromCodeCoverage]
public record DockerSwarmInitOptions : DockerOptions
{
    [CliOption("--advertise-addr")]
    public virtual string? AdvertiseAddr { get; set; }

    [CliOption("--autolock")]
    public virtual string? Autolock { get; set; }

    [CliOption("--availability")]
    public virtual string? Availability { get; set; }

    [CliOption("--cert-expiry")]
    public virtual string? CertExpiry { get; set; }

    [CliOption("--data-path-addr")]
    public virtual string? DataPathAddr { get; set; }

    [CliOption("--data-path-port")]
    public virtual string? DataPathPort { get; set; }

    [CliOption("--default-addr-pool")]
    public virtual string? DefaultAddrPool { get; set; }

    [CliOption("--default-addr-pool-mask-length")]
    public virtual int? DefaultAddrPoolMaskLength { get; set; }

    [CliOption("--dispatcher-heartbeat")]
    public virtual string? DispatcherHeartbeat { get; set; }

    [CliOption("--external-ca")]
    public virtual string? ExternalCa { get; set; }

    [CliOption("--force-new-cluster")]
    public virtual string? ForceNewCluster { get; set; }

    [CliOption("--listen-addr")]
    public virtual string? ListenAddr { get; set; }

    [CliOption("--max-snapshots")]
    public virtual string? MaxSnapshots { get; set; }

    [CliOption("--snapshot-interval")]
    public virtual int? SnapshotInterval { get; set; }

    [CliOption("--task-history-limit")]
    public virtual int? TaskHistoryLimit { get; set; }
}
