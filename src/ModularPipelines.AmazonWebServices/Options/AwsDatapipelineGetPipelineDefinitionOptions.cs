using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datapipeline", "get-pipeline-definition")]
public record AwsDatapipelineGetPipelineDefinitionOptions(
[property: CliOption("--pipeline-id")] string PipelineId
) : AwsOptions
{
    [CliOption("--pipeline-version")]
    public string? PipelineVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}