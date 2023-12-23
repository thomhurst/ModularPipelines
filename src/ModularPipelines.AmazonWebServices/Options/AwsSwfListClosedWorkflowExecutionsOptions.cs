using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("swf", "list-closed-workflow-executions")]
public record AwsSwfListClosedWorkflowExecutionsOptions(
[property: CommandSwitch("--domain")] string Domain
) : AwsOptions
{
    [CommandSwitch("--start-time-filter")]
    public string? StartTimeFilter { get; set; }

    [CommandSwitch("--close-time-filter")]
    public string? CloseTimeFilter { get; set; }

    [CommandSwitch("--execution-filter")]
    public string? ExecutionFilter { get; set; }

    [CommandSwitch("--close-status-filter")]
    public string? CloseStatusFilter { get; set; }

    [CommandSwitch("--type-filter")]
    public string? TypeFilter { get; set; }

    [CommandSwitch("--tag-filter")]
    public string? TagFilter { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}