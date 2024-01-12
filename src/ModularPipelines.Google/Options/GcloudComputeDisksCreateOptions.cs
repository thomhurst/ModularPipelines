using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "disks", "create")]
public record GcloudComputeDisksCreateOptions(
[property: PositionalArgument] string DiskName
) : GcloudOptions
{
    [CommandSwitch("--architecture")]
    public string? Architecture { get; set; }

    [BooleanCommandSwitch("--confidential-compute")]
    public bool? ConfidentialCompute { get; set; }

    [CommandSwitch("--csek-key-file")]
    public string? CsekKeyFile { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--guest-os-features")]
    public string[]? GuestOsFeatures { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--licenses")]
    public string[]? Licenses { get; set; }

    [CommandSwitch("--primary-disk-project")]
    public string? PrimaryDiskProject { get; set; }

    [CommandSwitch("--provisioned-iops")]
    public string? ProvisionedIops { get; set; }

    [CommandSwitch("--provisioned-throughput")]
    public string? ProvisionedThroughput { get; set; }

    [CommandSwitch("--replica-zones")]
    public string? ReplicaZones { get; set; }

    [BooleanCommandSwitch("--require-csek-key-create")]
    public bool? RequireCsekKeyCreate { get; set; }

    [CommandSwitch("--resource-policies")]
    public string[]? ResourcePolicies { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--image-family-scope")]
    public string? ImageFamilyScope { get; set; }

    [CommandSwitch("--image-project")]
    public string? ImageProject { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--image-family")]
    public string? ImageFamily { get; set; }

    [CommandSwitch("--primary-disk")]
    public string? PrimaryDisk { get; set; }

    [CommandSwitch("--source-disk")]
    public string? SourceDisk { get; set; }

    [CommandSwitch("--source-snapshot")]
    public string? SourceSnapshot { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-location")]
    public string? KmsLocation { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }

    [CommandSwitch("--primary-disk-region")]
    public string? PrimaryDiskRegion { get; set; }

    [CommandSwitch("--primary-disk-zone")]
    public string? PrimaryDiskZone { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--source-disk-region")]
    public string? SourceDiskRegion { get; set; }

    [CommandSwitch("--source-disk-zone")]
    public string? SourceDiskZone { get; set; }
}