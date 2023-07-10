using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm init")]
public record DockerSwarmInitOptions : DockerOptions
{
    [CommandLongSwitch("advertise-addr")]
    public string? AdvertiseAddr { get; set; }

    [CommandLongSwitch("autolock")]
    public string? Autolock { get; set; }

    [CommandLongSwitch("availability")]
    public string? Availability { get; set; }

    [CommandLongSwitch("cert-expiry")]
    public string? CertExpiry { get; set; }

    [CommandLongSwitch("data-path-addr")]
    public string? DataPathAddr { get; set; }

    [CommandLongSwitch("data-path-port")]
    public string? DataPathPort { get; set; }

    [CommandLongSwitch("default-addr-pool")]
    public string? DefaultAddrPool { get; set; }

    [CommandLongSwitch("default-addr-pool-mask-length")]
    public string? DefaultAddrPoolMaskLength { get; set; }

    [CommandLongSwitch("dispatcher-heartbeat")]
    public string? DispatcherHeartbeat { get; set; }

    [CommandLongSwitch("external-ca")]
    public string? ExternalCa { get; set; }

    [CommandLongSwitch("force-new-cluster")]
    public string? ForceNewCluster { get; set; }

    [CommandLongSwitch("listen-addr")]
    public string? ListenAddr { get; set; }

    [CommandLongSwitch("max-snapshots")]
    public string? MaxSnapshots { get; set; }

    [CommandLongSwitch("snapshot-interval")]
    public string? SnapshotInterval { get; set; }

    [CommandLongSwitch("task-history-limit")]
    public string? TaskHistoryLimit { get; set; }

}