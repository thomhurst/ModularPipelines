using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalytics", "delete-application-cloud-watch-logging-option")]
public record AwsKinesisanalyticsDeleteApplicationCloudWatchLoggingOptionOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--current-application-version-id")] long CurrentApplicationVersionId,
[property: CliOption("--cloud-watch-logging-option-id")] string CloudWatchLoggingOptionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}