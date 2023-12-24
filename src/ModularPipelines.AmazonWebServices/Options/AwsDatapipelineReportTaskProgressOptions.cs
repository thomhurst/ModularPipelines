using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datapipeline", "report-task-progress")]
public record AwsDatapipelineReportTaskProgressOptions(
[property: CommandSwitch("--task-id")] string TaskId
) : AwsOptions
{
    [CommandSwitch("--fields")]
    public string[]? Fields { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}