using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "update-provisioned-product-properties")]
public record AwsServicecatalogUpdateProvisionedProductPropertiesOptions(
[property: CommandSwitch("--provisioned-product-id")] string ProvisionedProductId,
[property: CommandSwitch("--provisioned-product-properties")] IEnumerable<KeyValue> ProvisionedProductProperties
) : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}