using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-pipeline")]
public record AwsSagemakerCreatePipelineOptions(
[property: CliOption("--pipeline-name")] string PipelineName,
[property: CliOption("--role-arn")] string RoleArn
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

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--parallelism-configuration")]
    public string? ParallelismConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}