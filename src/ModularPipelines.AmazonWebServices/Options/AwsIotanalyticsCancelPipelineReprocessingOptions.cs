using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotanalytics", "cancel-pipeline-reprocessing")]
public record AwsIotanalyticsCancelPipelineReprocessingOptions(
[property: CliOption("--pipeline-name")] string PipelineName,
[property: CliOption("--reprocessing-id")] string ReprocessingId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}