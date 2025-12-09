using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "respond-decision-task-completed")]
public record AwsSwfRespondDecisionTaskCompletedOptions(
[property: CliOption("--task-token")] string TaskToken
) : AwsOptions
{
    [CliOption("--decisions")]
    public string[]? Decisions { get; set; }

    [CliOption("--execution-context")]
    public string? ExecutionContext { get; set; }

    [CliOption("--task-list")]
    public string? TaskList { get; set; }

    [CliOption("--task-list-schedule-to-start-timeout")]
    public string? TaskListScheduleToStartTimeout { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}