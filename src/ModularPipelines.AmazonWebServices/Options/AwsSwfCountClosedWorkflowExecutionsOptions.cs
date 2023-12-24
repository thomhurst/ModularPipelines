using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("swf", "count-closed-workflow-executions")]
public record AwsSwfCountClosedWorkflowExecutionsOptions(
[property: CommandSwitch("--domain")] string Domain
) : AwsOptions
{
    [CommandSwitch("--start-time-filter")]
    public string? StartTimeFilter { get; set; }

    [CommandSwitch("--close-time-filter")]
    public string? CloseTimeFilter { get; set; }

    [CommandSwitch("--execution-filter")]
    public string? ExecutionFilter { get; set; }

    [CommandSwitch("--type-filter")]
    public string? TypeFilter { get; set; }

    [CommandSwitch("--tag-filter")]
    public string? TagFilter { get; set; }

    [CommandSwitch("--close-status-filter")]
    public string? CloseStatusFilter { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}