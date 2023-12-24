using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datapipeline", "list-runs")]
public record AwsDatapipelineListRunsOptions(
[property: CommandSwitch("--pipeline-id")] string PipelineId
) : AwsOptions
{
    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--start-interval")]
    public string? StartInterval { get; set; }

    [CommandSwitch("--schedule-interval")]
    public string? ScheduleInterval { get; set; }
}