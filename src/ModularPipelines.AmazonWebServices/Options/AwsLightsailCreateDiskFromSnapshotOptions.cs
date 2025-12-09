using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "create-disk-from-snapshot")]
public record AwsLightsailCreateDiskFromSnapshotOptions(
[property: CliOption("--disk-name")] string DiskName,
[property: CliOption("--availability-zone")] string AvailabilityZone,
[property: CliOption("--size-in-gb")] int SizeInGb
) : AwsOptions
{
    [CliOption("--disk-snapshot-name")]
    public string? DiskSnapshotName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--add-ons")]
    public string[]? AddOns { get; set; }

    [CliOption("--source-disk-name")]
    public string? SourceDiskName { get; set; }

    [CliOption("--restore-date")]
    public string? RestoreDate { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}