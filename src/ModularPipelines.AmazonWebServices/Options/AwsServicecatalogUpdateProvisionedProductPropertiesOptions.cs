using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "update-provisioned-product-properties")]
public record AwsServicecatalogUpdateProvisionedProductPropertiesOptions(
[property: CliOption("--provisioned-product-id")] string ProvisionedProductId,
[property: CliOption("--provisioned-product-properties")] IEnumerable<KeyValue> ProvisionedProductProperties
) : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}