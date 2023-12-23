using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-pipeline")]
public record AwsSagemakerCreatePipelineOptions(
[property: CommandSwitch("--pipeline-name")] string PipelineName,
[property: CommandSwitch("--role-arn")] string RoleArn
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

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--parallelism-configuration")]
    public string? ParallelismConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}