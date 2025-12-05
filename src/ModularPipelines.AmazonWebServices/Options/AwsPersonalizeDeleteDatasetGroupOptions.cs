using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "delete-dataset-group")]
public record AwsPersonalizeDeleteDatasetGroupOptions(
[property: CliOption("--dataset-group-arn")] string DatasetGroupArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}