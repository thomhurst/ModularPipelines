using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("swf", "list-open-workflow-executions")]
public record AwsSwfListOpenWorkflowExecutionsOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--start-time-filter")] string StartTimeFilter
) : AwsOptions
{
    [CommandSwitch("--type-filter")]
    public string? TypeFilter { get; set; }

    [CommandSwitch("--tag-filter")]
    public string? TagFilter { get; set; }

    [CommandSwitch("--execution-filter")]
    public string? ExecutionFilter { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}