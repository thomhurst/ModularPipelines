using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "create-provisioned-product-plan")]
public record AwsServicecatalogCreateProvisionedProductPlanOptions(
[property: CliOption("--plan-name")] string PlanName,
[property: CliOption("--plan-type")] string PlanType,
[property: CliOption("--product-id")] string ProductId,
[property: CliOption("--provisioned-product-name")] string ProvisionedProductName,
[property: CliOption("--provisioning-artifact-id")] string ProvisioningArtifactId
) : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--notification-arns")]
    public string[]? NotificationArns { get; set; }

    [CliOption("--path-id")]
    public string? PathId { get; set; }

    [CliOption("--provisioning-parameters")]
    public string[]? ProvisioningParameters { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}