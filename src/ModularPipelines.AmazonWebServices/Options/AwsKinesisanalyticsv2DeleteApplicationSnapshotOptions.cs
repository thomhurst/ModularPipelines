using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalyticsv2", "delete-application-snapshot")]
public record AwsKinesisanalyticsv2DeleteApplicationSnapshotOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--snapshot-name")] string SnapshotName,
[property: CliOption("--snapshot-creation-timestamp")] long SnapshotCreationTimestamp
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}