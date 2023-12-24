using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("swf", "terminate-workflow-execution")]
public record AwsSwfTerminateWorkflowExecutionOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--workflow-id")] string WorkflowId
) : AwsOptions
{
    [CommandSwitch("--run-id")]
    public string? RunId { get; set; }

    [CommandSwitch("--reason")]
    public string? Reason { get; set; }

    [CommandSwitch("--details")]
    public string? Details { get; set; }

    [CommandSwitch("--child-policy")]
    public string? ChildPolicy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}