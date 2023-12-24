using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "start-pipeline-execution")]
public record AwsSagemakerStartPipelineExecutionOptions(
[property: CommandSwitch("--pipeline-name")] string PipelineName
) : AwsOptions
{
    [CommandSwitch("--pipeline-execution-display-name")]
    public string? PipelineExecutionDisplayName { get; set; }

    [CommandSwitch("--pipeline-parameters")]
    public string[]? PipelineParameters { get; set; }

    [CommandSwitch("--pipeline-execution-description")]
    public string? PipelineExecutionDescription { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--parallelism-configuration")]
    public string? ParallelismConfiguration { get; set; }

    [CommandSwitch("--selective-execution-config")]
    public string? SelectiveExecutionConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}