using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-pipeline-execution")]
public record AwsSagemakerUpdatePipelineExecutionOptions(
[property: CommandSwitch("--pipeline-execution-arn")] string PipelineExecutionArn
) : AwsOptions
{
    [CommandSwitch("--pipeline-execution-description")]
    public string? PipelineExecutionDescription { get; set; }

    [CommandSwitch("--pipeline-execution-display-name")]
    public string? PipelineExecutionDisplayName { get; set; }

    [CommandSwitch("--parallelism-configuration")]
    public string? ParallelismConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}