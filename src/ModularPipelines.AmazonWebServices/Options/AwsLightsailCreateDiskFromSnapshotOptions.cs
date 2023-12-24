using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "create-disk-from-snapshot")]
public record AwsLightsailCreateDiskFromSnapshotOptions(
[property: CommandSwitch("--disk-name")] string DiskName,
[property: CommandSwitch("--availability-zone")] string AvailabilityZone,
[property: CommandSwitch("--size-in-gb")] int SizeInGb
) : AwsOptions
{
    [CommandSwitch("--disk-snapshot-name")]
    public string? DiskSnapshotName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--add-ons")]
    public string[]? AddOns { get; set; }

    [CommandSwitch("--source-disk-name")]
    public string? SourceDiskName { get; set; }

    [CommandSwitch("--restore-date")]
    public string? RestoreDate { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}