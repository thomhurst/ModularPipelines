using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "copy-snapshot")]
public record AwsLightsailCopySnapshotOptions(
[property: CliOption("--target-snapshot-name")] string TargetSnapshotName,
[property: CliOption("--source-region")] string SourceRegion
) : AwsOptions
{
    [CliOption("--source-snapshot-name")]
    public string? SourceSnapshotName { get; set; }

    [CliOption("--source-resource-name")]
    public string? SourceResourceName { get; set; }

    [CliOption("--restore-date")]
    public string? RestoreDate { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}