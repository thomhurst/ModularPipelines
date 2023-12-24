using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("swf", "register-workflow-type")]
public record AwsSwfRegisterWorkflowTypeOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--workflow-version")] string WorkflowVersion
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--default-task-start-to-close-timeout")]
    public string? DefaultTaskStartToCloseTimeout { get; set; }

    [CommandSwitch("--default-execution-start-to-close-timeout")]
    public string? DefaultExecutionStartToCloseTimeout { get; set; }

    [CommandSwitch("--default-task-list")]
    public string? DefaultTaskList { get; set; }

    [CommandSwitch("--default-task-priority")]
    public string? DefaultTaskPriority { get; set; }

    [CommandSwitch("--default-child-policy")]
    public string? DefaultChildPolicy { get; set; }

    [CommandSwitch("--default-lambda-role")]
    public string? DefaultLambdaRole { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}