using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "import-as-provisioned-product")]
public record AwsServicecatalogImportAsProvisionedProductOptions(
[property: CliOption("--product-id")] string ProductId,
[property: CliOption("--provisioning-artifact-id")] string ProvisioningArtifactId,
[property: CliOption("--provisioned-product-name")] string ProvisionedProductName,
[property: CliOption("--physical-id")] string PhysicalId
) : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}