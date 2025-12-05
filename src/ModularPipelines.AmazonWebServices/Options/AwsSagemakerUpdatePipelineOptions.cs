using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-pipeline")]
public record AwsSagemakerUpdatePipelineOptions(
[property: CliOption("--pipeline-name")] string PipelineName
) : AwsOptions
{
    [CliOption("--pipeline-display-name")]
    public string? PipelineDisplayName { get; set; }

    [CliOption("--pipeline-definition")]
    public string? PipelineDefinition { get; set; }

    [CliOption("--pipeline-definition-s3-location")]
    public string? PipelineDefinitionS3Location { get; set; }

    [CliOption("--pipeline-description")]
    public string? PipelineDescription { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--parallelism-configuration")]
    public string? ParallelismConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}