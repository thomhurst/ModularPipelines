using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalytics", "delete-application-reference-data-source")]
public record AwsKinesisanalyticsDeleteApplicationReferenceDataSourceOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--current-application-version-id")] long CurrentApplicationVersionId,
[property: CliOption("--reference-id")] string ReferenceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}