using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "describe-dataset-import-job")]
public record AwsPersonalizeDescribeDatasetImportJobOptions(
[property: CommandSwitch("--dataset-import-job-arn")] string DatasetImportJobArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}