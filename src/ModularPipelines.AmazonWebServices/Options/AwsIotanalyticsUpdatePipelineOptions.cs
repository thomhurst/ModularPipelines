using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotanalytics", "update-pipeline")]
public record AwsIotanalyticsUpdatePipelineOptions(
[property: CliOption("--pipeline-name")] string PipelineName,
[property: CliOption("--pipeline-activities")] string[] PipelineActivities
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}