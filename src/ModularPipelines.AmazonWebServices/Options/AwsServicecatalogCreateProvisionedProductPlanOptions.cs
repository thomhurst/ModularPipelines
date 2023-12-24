using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "create-provisioned-product-plan")]
public record AwsServicecatalogCreateProvisionedProductPlanOptions(
[property: CommandSwitch("--plan-name")] string PlanName,
[property: CommandSwitch("--plan-type")] string PlanType,
[property: CommandSwitch("--product-id")] string ProductId,
[property: CommandSwitch("--provisioned-product-name")] string ProvisionedProductName,
[property: CommandSwitch("--provisioning-artifact-id")] string ProvisioningArtifactId
) : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--notification-arns")]
    public string[]? NotificationArns { get; set; }

    [CommandSwitch("--path-id")]
    public string? PathId { get; set; }

    [CommandSwitch("--provisioning-parameters")]
    public string[]? ProvisioningParameters { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}