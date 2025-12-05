using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutvision", "describe-model")]
public record AwsLookoutvisionDescribeModelOptions(
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--model-version")] string ModelVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}