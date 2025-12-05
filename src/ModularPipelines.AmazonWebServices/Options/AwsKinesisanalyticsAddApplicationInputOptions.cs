using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalytics", "add-application-input")]
public record AwsKinesisanalyticsAddApplicationInputOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--current-application-version-id")] long CurrentApplicationVersionId,
[property: CliOption("--input")] string Input
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}