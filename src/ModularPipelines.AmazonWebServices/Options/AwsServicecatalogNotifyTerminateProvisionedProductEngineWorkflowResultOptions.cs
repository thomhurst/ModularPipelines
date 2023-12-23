using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "notify-terminate-provisioned-product-engine-workflow-result")]
public record AwsServicecatalogNotifyTerminateProvisionedProductEngineWorkflowResultOptions(
[property: CommandSwitch("--workflow-token")] string WorkflowToken,
[property: CommandSwitch("--record-id")] string RecordId,
[property: CommandSwitch("--status")] string Status
) : AwsOptions
{
    [CommandSwitch("--failure-reason")]
    public string? FailureReason { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}