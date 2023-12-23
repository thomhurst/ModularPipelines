using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "describe-pipeline-definition-for-execution")]
public record AwsSagemakerDescribePipelineDefinitionForExecutionOptions(
[property: CommandSwitch("--pipeline-execution-arn")] string PipelineExecutionArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}