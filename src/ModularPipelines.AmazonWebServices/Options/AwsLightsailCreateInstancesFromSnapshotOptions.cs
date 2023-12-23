using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "create-instances-from-snapshot")]
public record AwsLightsailCreateInstancesFromSnapshotOptions(
[property: CommandSwitch("--instance-names")] string[] InstanceNames,
[property: CommandSwitch("--availability-zone")] string AvailabilityZone,
[property: CommandSwitch("--bundle-id")] string BundleId
) : AwsOptions
{
    [CommandSwitch("--attached-disk-mapping")]
    public IEnumerable<KeyValue>? AttachedDiskMapping { get; set; }

    [CommandSwitch("--instance-snapshot-name")]
    public string? InstanceSnapshotName { get; set; }

    [CommandSwitch("--user-data")]
    public string? UserData { get; set; }

    [CommandSwitch("--key-pair-name")]
    public string? KeyPairName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--add-ons")]
    public string[]? AddOns { get; set; }

    [CommandSwitch("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CommandSwitch("--source-instance-name")]
    public string? SourceInstanceName { get; set; }

    [CommandSwitch("--restore-date")]
    public string? RestoreDate { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}