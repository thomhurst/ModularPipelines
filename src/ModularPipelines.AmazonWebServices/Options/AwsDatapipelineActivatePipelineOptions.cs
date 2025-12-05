using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datapipeline", "activate-pipeline")]
public record AwsDatapipelineActivatePipelineOptions(
[property: CliOption("--pipeline-id")] string PipelineId
) : AwsOptions
{
    [CliOption("--parameter-values")]
    public string? ParameterValues { get; set; }

    [CliOption("--start-timestamp")]
    public long? StartTimestamp { get; set; }

    [CliOption("--parameter-values-uri")]
    public string? ParameterValuesUri { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}