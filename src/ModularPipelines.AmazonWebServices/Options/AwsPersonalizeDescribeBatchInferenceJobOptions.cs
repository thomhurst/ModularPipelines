using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "describe-batch-inference-job")]
public record AwsPersonalizeDescribeBatchInferenceJobOptions(
[property: CommandSwitch("--batch-inference-job-arn")] string BatchInferenceJobArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}