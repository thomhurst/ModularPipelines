using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "create-disk-snapshot")]
public record AwsLightsailCreateDiskSnapshotOptions(
[property: CliOption("--disk-snapshot-name")] string DiskSnapshotName
) : AwsOptions
{
    [CliOption("--disk-name")]
    public string? DiskName { get; set; }

    [CliOption("--instance-name")]
    public string? InstanceName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}