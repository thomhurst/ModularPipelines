using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("swf", "start-workflow-execution")]
public record AwsSwfStartWorkflowExecutionOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--workflow-id")] string WorkflowId,
[property: CommandSwitch("--workflow-type")] string WorkflowType
) : AwsOptions
{
    [CommandSwitch("--task-list")]
    public string? TaskList { get; set; }

    [CommandSwitch("--task-priority")]
    public string? TaskPriority { get; set; }

    [CommandSwitch("--input")]
    public string? Input { get; set; }

    [CommandSwitch("--execution-start-to-close-timeout")]
    public string? ExecutionStartToCloseTimeout { get; set; }

    [CommandSwitch("--tag-list")]
    public string[]? TagList { get; set; }

    [CommandSwitch("--task-start-to-close-timeout")]
    public string? TaskStartToCloseTimeout { get; set; }

    [CommandSwitch("--child-policy")]
    public string? ChildPolicy { get; set; }

    [CommandSwitch("--lambda-role")]
    public string? LambdaRole { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}