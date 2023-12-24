using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("swf", "count-open-workflow-executions")]
public record AwsSwfCountOpenWorkflowExecutionsOptions(
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

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}