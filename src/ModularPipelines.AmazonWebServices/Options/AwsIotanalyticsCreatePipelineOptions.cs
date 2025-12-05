using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotanalytics", "create-pipeline")]
public record AwsIotanalyticsCreatePipelineOptions(
[property: CliOption("--pipeline-name")] string PipelineName,
[property: CliOption("--pipeline-activities")] string[] PipelineActivities
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}