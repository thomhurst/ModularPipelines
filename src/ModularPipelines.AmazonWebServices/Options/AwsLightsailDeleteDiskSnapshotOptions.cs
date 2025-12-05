using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "delete-disk-snapshot")]
public record AwsLightsailDeleteDiskSnapshotOptions(
[property: CliOption("--disk-snapshot-name")] string DiskSnapshotName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}