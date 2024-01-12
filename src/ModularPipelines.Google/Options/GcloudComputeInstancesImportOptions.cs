using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "import")]
public record GcloudComputeInstancesImportOptions(
[property: PositionalArgument] string InstanceName,
[property: CommandSwitch("--source-uri")] string SourceUri
) : GcloudOptions
{
    [BooleanCommandSwitch("--no-address")]
    public bool? NoAddress { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--byol")]
    public bool? Byol { get; set; }

    [BooleanCommandSwitch("--can-ip-forward")]
    public bool? CanIpForward { get; set; }

    [CommandSwitch("--cloudbuild-service-account")]
    public string? CloudbuildServiceAccount { get; set; }

    [CommandSwitch("--compute-service-account")]
    public string? ComputeServiceAccount { get; set; }

    [BooleanCommandSwitch("--deletion-protection")]
    public bool? DeletionProtection { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--guest-environment")]
    public bool? GuestEnvironment { get; set; }

    [CommandSwitch("--guest-os-features")]
    public string[]? GuestOsFeatures { get; set; }

    [CommandSwitch("--hostname")]
    public string? Hostname { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--log-location")]
    public string? LogLocation { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--network-tier")]
    public string? NetworkTier { get; set; }

    [CommandSwitch("--os")]
    public string? Os { get; set; }

    [CommandSwitch("--private-network-ip")]
    public string? PrivateNetworkIp { get; set; }

    [BooleanCommandSwitch("--restart-on-failure")]
    public bool? RestartOnFailure { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--custom-cpu")]
    public string? CustomCpu { get; set; }

    [CommandSwitch("--custom-memory")]
    public string? CustomMemory { get; set; }

    [BooleanCommandSwitch("--custom-extensions")]
    public bool? CustomExtensions { get; set; }

    [CommandSwitch("--custom-vm-type")]
    public string? CustomVmType { get; set; }

    [CommandSwitch("--node")]
    public string? Node { get; set; }

    [CommandSwitch("--node-affinity-file")]
    public string? NodeAffinityFile { get; set; }

    [BooleanCommandSwitch("key")]
    public bool? Key { get; set; }

    [BooleanCommandSwitch("operator")]
    public bool? Operator { get; set; }

    [BooleanCommandSwitch("values")]
    public bool? Values { get; set; }

    [CommandSwitch("--node-group")]
    public string? NodeGroup { get; set; }

    [CommandSwitch("--scopes")]
    public string[]? Scopes { get; set; }

    [BooleanCommandSwitch("--no-scopes")]
    public bool? NoScopes { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [BooleanCommandSwitch("--no-service-account")]
    public bool? NoServiceAccount { get; set; }
}