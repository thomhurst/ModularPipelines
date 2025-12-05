using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalyticsv2", "create-application-snapshot")]
public record AwsKinesisanalyticsv2CreateApplicationSnapshotOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--snapshot-name")] string SnapshotName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}