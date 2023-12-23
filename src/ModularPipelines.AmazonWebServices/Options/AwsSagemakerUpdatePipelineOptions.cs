using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-pipeline")]
public record AwsSagemakerUpdatePipelineOptions(
[property: CommandSwitch("--pipeline-name")] string PipelineName
) : AwsOptions
{
    [CommandSwitch("--pipeline-display-name")]
    public string? PipelineDisplayName { get; set; }

    [CommandSwitch("--pipeline-definition")]
    public string? PipelineDefinition { get; set; }

    [CommandSwitch("--pipeline-definition-s3-location")]
    public string? PipelineDefinitionS3Location { get; set; }

    [CommandSwitch("--pipeline-description")]
    public string? PipelineDescription { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--parallelism-configuration")]
    public string? ParallelismConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}