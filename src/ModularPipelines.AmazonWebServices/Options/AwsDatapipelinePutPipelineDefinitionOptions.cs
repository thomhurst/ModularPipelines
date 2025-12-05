using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datapipeline", "put-pipeline-definition")]
public record AwsDatapipelinePutPipelineDefinitionOptions(
[property: CliOption("--pipeline-id")] string PipelineId,
[property: CliOption("--pipeline-definition")] string PipelineDefinition
) : AwsOptions
{
    [CliOption("--parameter-objects")]
    public string? ParameterObjects { get; set; }

    [CliOption("--parameter-values")]
    public string? ParameterValues { get; set; }

    [CliOption("--parameter-values-uri")]
    public string? ParameterValuesUri { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}