using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "create-instances-from-snapshot")]
public record AwsLightsailCreateInstancesFromSnapshotOptions(
[property: CliOption("--instance-names")] string[] InstanceNames,
[property: CliOption("--availability-zone")] string AvailabilityZone,
[property: CliOption("--bundle-id")] string BundleId
) : AwsOptions
{
    [CliOption("--attached-disk-mapping")]
    public IEnumerable<KeyValue>? AttachedDiskMapping { get; set; }

    [CliOption("--instance-snapshot-name")]
    public string? InstanceSnapshotName { get; set; }

    [CliOption("--user-data")]
    public string? UserData { get; set; }

    [CliOption("--key-pair-name")]
    public string? KeyPairName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--add-ons")]
    public string[]? AddOns { get; set; }

    [CliOption("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CliOption("--source-instance-name")]
    public string? SourceInstanceName { get; set; }

    [CliOption("--restore-date")]
    public string? RestoreDate { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}