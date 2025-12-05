using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datapipeline", "deactivate-pipeline")]
public record AwsDatapipelineDeactivatePipelineOptions(
[property: CliOption("--pipeline-id")] string PipelineId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}