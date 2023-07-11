using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm init")]
public record DockerSwarmInitOptions : DockerOptions
{

    [CommandSwitch("--advertise-addr")]
    public string? AdvertiseAddr { get; set; }


    [CommandSwitch("--autolock")]
    public string? Autolock { get; set; }


    [CommandSwitch("--availability")]
    public string? Availability { get; set; }


    [CommandSwitch("--cert-expiry")]
    public string? CertExpiry { get; set; }


    [CommandSwitch("--data-path-addr")]
    public string? DataPathAddr { get; set; }


    [CommandSwitch("--data-path-port")]
    public string? DataPathPort { get; set; }


    [CommandSwitch("--default-addr-pool")]
    public string? DefaultAddrPool { get; set; }


    [CommandSwitch("--default-addr-pool-mask-length")]
    public int? DefaultAddrPoolMaskLength { get; set; }


    [CommandSwitch("--dispatcher-heartbeat")]
    public string? DispatcherHeartbeat { get; set; }


    [CommandSwitch("--external-ca")]
    public string? ExternalCa { get; set; }


    [CommandSwitch("--force-new-cluster")]
    public string? ForceNewCluster { get; set; }


    [CommandSwitch("--listen-addr")]
    public string? ListenAddr { get; set; }


    [CommandSwitch("--max-snapshots")]
    public string? MaxSnapshots { get; set; }


    [CommandSwitch("--snapshot-interval")]
    public int? SnapshotInterval { get; set; }


    [CommandSwitch("--task-history-limit")]
    public int? TaskHistoryLimit { get; set; }

}