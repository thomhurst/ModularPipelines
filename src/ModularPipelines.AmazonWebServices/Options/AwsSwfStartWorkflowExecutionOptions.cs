using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "start-workflow-execution")]
public record AwsSwfStartWorkflowExecutionOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--workflow-id")] string WorkflowId,
[property: CliOption("--workflow-type")] string WorkflowType
) : AwsOptions
{
    [CliOption("--task-list")]
    public string? TaskList { get; set; }

    [CliOption("--task-priority")]
    public string? TaskPriority { get; set; }

    [CliOption("--input")]
    public string? Input { get; set; }

    [CliOption("--execution-start-to-close-timeout")]
    public string? ExecutionStartToCloseTimeout { get; set; }

    [CliOption("--tag-list")]
    public string[]? TagList { get; set; }

    [CliOption("--task-start-to-close-timeout")]
    public string? TaskStartToCloseTimeout { get; set; }

    [CliOption("--child-policy")]
    public string? ChildPolicy { get; set; }

    [CliOption("--lambda-role")]
    public string? LambdaRole { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}