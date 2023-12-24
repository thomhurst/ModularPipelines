using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "describe-dataset-import-job")]
public record AwsForecastDescribeDatasetImportJobOptions(
[property: CommandSwitch("--dataset-import-job-arn")] string DatasetImportJobArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}