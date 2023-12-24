using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "batch-describe-model-package")]
public record AwsSagemakerBatchDescribeModelPackageOptions(
[property: CommandSwitch("--model-package-arn-list")] string[] ModelPackageArnList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}