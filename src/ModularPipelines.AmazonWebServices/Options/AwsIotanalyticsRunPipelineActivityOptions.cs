using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotanalytics", "run-pipeline-activity")]
public record AwsIotanalyticsRunPipelineActivityOptions(
[property: CliOption("--pipeline-activity")] string PipelineActivity,
[property: CliOption("--payloads")] string[] Payloads
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}