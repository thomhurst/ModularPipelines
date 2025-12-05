using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutvision", "detect-anomalies")]
public record AwsLookoutvisionDetectAnomaliesOptions(
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--model-version")] string ModelVersion,
[property: CliOption("--body")] string Body,
[property: CliOption("--content-type")] string ContentType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}