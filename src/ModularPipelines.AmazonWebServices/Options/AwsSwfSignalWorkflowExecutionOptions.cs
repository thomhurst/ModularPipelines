using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("swf", "signal-workflow-execution")]
public record AwsSwfSignalWorkflowExecutionOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--workflow-id")] string WorkflowId,
[property: CommandSwitch("--signal-name")] string SignalName
) : AwsOptions
{
    [CommandSwitch("--run-id")]
    public string? RunId { get; set; }

    [CommandSwitch("--input")]
    public string? Input { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}