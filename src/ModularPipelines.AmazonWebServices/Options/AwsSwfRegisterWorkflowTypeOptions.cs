using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "register-workflow-type")]
public record AwsSwfRegisterWorkflowTypeOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--name")] string Name,
[property: CliOption("--workflow-version")] string WorkflowVersion
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--default-task-start-to-close-timeout")]
    public string? DefaultTaskStartToCloseTimeout { get; set; }

    [CliOption("--default-execution-start-to-close-timeout")]
    public string? DefaultExecutionStartToCloseTimeout { get; set; }

    [CliOption("--default-task-list")]
    public string? DefaultTaskList { get; set; }

    [CliOption("--default-task-priority")]
    public string? DefaultTaskPriority { get; set; }

    [CliOption("--default-child-policy")]
    public string? DefaultChildPolicy { get; set; }

    [CliOption("--default-lambda-role")]
    public string? DefaultLambdaRole { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}