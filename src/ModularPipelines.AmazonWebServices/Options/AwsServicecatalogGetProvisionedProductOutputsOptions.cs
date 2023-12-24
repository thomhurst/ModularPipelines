using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "get-provisioned-product-outputs")]
public record AwsServicecatalogGetProvisionedProductOutputsOptions : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--provisioned-product-id")]
    public string? ProvisionedProductId { get; set; }

    [CommandSwitch("--provisioned-product-name")]
    public string? ProvisionedProductName { get; set; }

    [CommandSwitch("--output-keys")]
    public string[]? OutputKeys { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--page-token")]
    public string? PageToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}