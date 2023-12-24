using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("swf", "respond-decision-task-completed")]
public record AwsSwfRespondDecisionTaskCompletedOptions(
[property: CommandSwitch("--task-token")] string TaskToken
) : AwsOptions
{
    [CommandSwitch("--decisions")]
    public string[]? Decisions { get; set; }

    [CommandSwitch("--execution-context")]
    public string? ExecutionContext { get; set; }

    [CommandSwitch("--task-list")]
    public string? TaskList { get; set; }

    [CommandSwitch("--task-list-schedule-to-start-timeout")]
    public string? TaskListScheduleToStartTimeout { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}