using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalyticsv2", "add-application-input")]
public record AwsKinesisanalyticsv2AddApplicationInputOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--current-application-version-id")] long CurrentApplicationVersionId,
[property: CliOption("--input")] string Input
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}