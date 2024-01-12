using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workstations", "configs", "create")]
public record GcloudWorkstationsConfigsCreateOptions(
[property: PositionalArgument] string Config,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--boot-disk-size")]
    public string? BootDiskSize { get; set; }

    [CommandSwitch("--container-args")]
    public string[]? ContainerArgs { get; set; }

    [CommandSwitch("--container-command")]
    public string[]? ContainerCommand { get; set; }

    [CommandSwitch("--container-env")]
    public string[]? ContainerEnv { get; set; }

    [CommandSwitch("--container-run-as-user")]
    public string? ContainerRunAsUser { get; set; }

    [CommandSwitch("--container-working-dir")]
    public string? ContainerWorkingDir { get; set; }

    [BooleanCommandSwitch("--disable-public-ip-addresses")]
    public bool? DisablePublicIpAddresses { get; set; }

    [BooleanCommandSwitch("--enable-audit-agent")]
    public bool? EnableAuditAgent { get; set; }

    [BooleanCommandSwitch("--enable-confidential-compute")]
    public bool? EnableConfidentialCompute { get; set; }

    [BooleanCommandSwitch("--enable-nested-virtualization")]
    public bool? EnableNestedVirtualization { get; set; }

    [CommandSwitch("--idle-timeout")]
    public string? IdleTimeout { get; set; }

    [CommandSwitch("--labels")]
    public string[]? Labels { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--network-tags")]
    public string[]? NetworkTags { get; set; }

    [CommandSwitch("--pd-disk-size")]
    public string? PdDiskSize { get; set; }

    [CommandSwitch("--pd-disk-type")]
    public string? PdDiskType { get; set; }

    [CommandSwitch("--pd-reclaim-policy")]
    public string? PdReclaimPolicy { get; set; }

    [CommandSwitch("--pool-size")]
    public string? PoolSize { get; set; }

    [CommandSwitch("--replica-zones")]
    public string[]? ReplicaZones { get; set; }

    [CommandSwitch("--running-timeout")]
    public string? RunningTimeout { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--service-account-scopes")]
    public string[]? ServiceAccountScopes { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-key-service-account")]
    public string? KmsKeyServiceAccount { get; set; }
}