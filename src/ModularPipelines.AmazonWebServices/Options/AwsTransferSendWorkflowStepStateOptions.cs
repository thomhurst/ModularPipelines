using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "send-workflow-step-state")]
public record AwsTransferSendWorkflowStepStateOptions(
[property: CliOption("--workflow-id")] string WorkflowId,
[property: CliOption("--execution-id")] string ExecutionId,
[property: CliOption("--token")] string Token,
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}