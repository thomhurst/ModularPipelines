using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datapipeline", "list-runs")]
public record AwsDatapipelineListRunsOptions(
[property: CliOption("--pipeline-id")] string PipelineId
) : AwsOptions
{
    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--start-interval")]
    public string? StartInterval { get; set; }

    [CliOption("--schedule-interval")]
    public string? ScheduleInterval { get; set; }
}