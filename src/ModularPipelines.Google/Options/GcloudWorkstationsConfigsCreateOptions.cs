using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workstations", "configs", "create")]
public record GcloudWorkstationsConfigsCreateOptions(
[property: CliArgument] string Config,
[property: CliArgument] string Cluster,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--boot-disk-size")]
    public string? BootDiskSize { get; set; }

    [CliOption("--container-args")]
    public string[]? ContainerArgs { get; set; }

    [CliOption("--container-command")]
    public string[]? ContainerCommand { get; set; }

    [CliOption("--container-env")]
    public string[]? ContainerEnv { get; set; }

    [CliOption("--container-run-as-user")]
    public string? ContainerRunAsUser { get; set; }

    [CliOption("--container-working-dir")]
    public string? ContainerWorkingDir { get; set; }

    [CliFlag("--disable-public-ip-addresses")]
    public bool? DisablePublicIpAddresses { get; set; }

    [CliFlag("--enable-audit-agent")]
    public bool? EnableAuditAgent { get; set; }

    [CliFlag("--enable-confidential-compute")]
    public bool? EnableConfidentialCompute { get; set; }

    [CliFlag("--enable-nested-virtualization")]
    public bool? EnableNestedVirtualization { get; set; }

    [CliOption("--idle-timeout")]
    public string? IdleTimeout { get; set; }

    [CliOption("--labels")]
    public string[]? Labels { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--network-tags")]
    public string[]? NetworkTags { get; set; }

    [CliOption("--pd-disk-size")]
    public string? PdDiskSize { get; set; }

    [CliOption("--pd-disk-type")]
    public string? PdDiskType { get; set; }

    [CliOption("--pd-reclaim-policy")]
    public string? PdReclaimPolicy { get; set; }

    [CliOption("--pool-size")]
    public string? PoolSize { get; set; }

    [CliOption("--replica-zones")]
    public string[]? ReplicaZones { get; set; }

    [CliOption("--running-timeout")]
    public string? RunningTimeout { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliOption("--service-account-scopes")]
    public string[]? ServiceAccountScopes { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--kms-key-service-account")]
    public string? KmsKeyServiceAccount { get; set; }
}