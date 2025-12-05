using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalyticsv2", "delete-application-cloud-watch-logging-option")]
public record AwsKinesisanalyticsv2DeleteApplicationCloudWatchLoggingOptionOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--cloud-watch-logging-option-id")] string CloudWatchLoggingOptionId
) : AwsOptions
{
    [CliOption("--current-application-version-id")]
    public long? CurrentApplicationVersionId { get; set; }

    [CliOption("--conditional-token")]
    public string? ConditionalToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}