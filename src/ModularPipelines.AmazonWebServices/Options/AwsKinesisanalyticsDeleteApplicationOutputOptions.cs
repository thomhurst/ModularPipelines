using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalytics", "delete-application-output")]
public record AwsKinesisanalyticsDeleteApplicationOutputOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--current-application-version-id")] long CurrentApplicationVersionId,
[property: CliOption("--output-id")] string OutputId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}