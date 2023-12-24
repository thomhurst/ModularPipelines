using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "send-workflow-step-state")]
public record AwsTransferSendWorkflowStepStateOptions(
[property: CommandSwitch("--workflow-id")] string WorkflowId,
[property: CommandSwitch("--execution-id")] string ExecutionId,
[property: CommandSwitch("--token")] string Token,
[property: CommandSwitch("--status")] string Status
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}