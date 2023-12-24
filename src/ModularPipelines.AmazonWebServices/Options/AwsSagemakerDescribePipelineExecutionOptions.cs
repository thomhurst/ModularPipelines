using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "describe-pipeline-execution")]
public record AwsSagemakerDescribePipelineExecutionOptions(
[property: CommandSwitch("--pipeline-execution-arn")] string PipelineExecutionArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}