using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "get-provisioned-product-outputs")]
public record AwsServicecatalogGetProvisionedProductOutputsOptions : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--provisioned-product-id")]
    public string? ProvisionedProductId { get; set; }

    [CliOption("--provisioned-product-name")]
    public string? ProvisionedProductName { get; set; }

    [CliOption("--output-keys")]
    public string[]? OutputKeys { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--page-token")]
    public string? PageToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}