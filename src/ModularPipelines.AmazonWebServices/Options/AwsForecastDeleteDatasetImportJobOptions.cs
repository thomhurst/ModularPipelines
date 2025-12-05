using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "delete-dataset-import-job")]
public record AwsForecastDeleteDatasetImportJobOptions(
[property: CliOption("--dataset-import-job-arn")] string DatasetImportJobArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}