using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "disks", "create")]
public record GcloudComputeDisksCreateOptions(
[property: CliArgument] string DiskName
) : GcloudOptions
{
    [CliOption("--architecture")]
    public string? Architecture { get; set; }

    [CliFlag("--confidential-compute")]
    public bool? ConfidentialCompute { get; set; }

    [CliOption("--csek-key-file")]
    public string? CsekKeyFile { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--guest-os-features")]
    public string[]? GuestOsFeatures { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--licenses")]
    public string[]? Licenses { get; set; }

    [CliOption("--primary-disk-project")]
    public string? PrimaryDiskProject { get; set; }

    [CliOption("--provisioned-iops")]
    public string? ProvisionedIops { get; set; }

    [CliOption("--provisioned-throughput")]
    public string? ProvisionedThroughput { get; set; }

    [CliOption("--replica-zones")]
    public string? ReplicaZones { get; set; }

    [CliFlag("--require-csek-key-create")]
    public bool? RequireCsekKeyCreate { get; set; }

    [CliOption("--resource-policies")]
    public string[]? ResourcePolicies { get; set; }

    [CliOption("--size")]
    public string? Size { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--image-family-scope")]
    public string? ImageFamilyScope { get; set; }

    [CliOption("--image-project")]
    public string? ImageProject { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--image-family")]
    public string? ImageFamily { get; set; }

    [CliOption("--primary-disk")]
    public string? PrimaryDisk { get; set; }

    [CliOption("--source-disk")]
    public string? SourceDisk { get; set; }

    [CliOption("--source-snapshot")]
    public string? SourceSnapshot { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-location")]
    public string? KmsLocation { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }

    [CliOption("--primary-disk-region")]
    public string? PrimaryDiskRegion { get; set; }

    [CliOption("--primary-disk-zone")]
    public string? PrimaryDiskZone { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--source-disk-region")]
    public string? SourceDiskRegion { get; set; }

    [CliOption("--source-disk-zone")]
    public string? SourceDiskZone { get; set; }
}