using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutvision", "describe-dataset")]
public record AwsLookoutvisionDescribeDatasetOptions(
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--dataset-type")] string DatasetType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}