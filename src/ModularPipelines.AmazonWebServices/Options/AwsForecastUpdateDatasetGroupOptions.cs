using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "update-dataset-group")]
public record AwsForecastUpdateDatasetGroupOptions(
[property: CliOption("--dataset-group-arn")] string DatasetGroupArn,
[property: CliOption("--dataset-arns")] string[] DatasetArns
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}