using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "notify-update-provisioned-product-engine-workflow-result")]
public record AwsServicecatalogNotifyUpdateProvisionedProductEngineWorkflowResultOptions(
[property: CliOption("--workflow-token")] string WorkflowToken,
[property: CliOption("--record-id")] string RecordId,
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--failure-reason")]
    public string? FailureReason { get; set; }

    [CliOption("--outputs")]
    public string[]? Outputs { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}