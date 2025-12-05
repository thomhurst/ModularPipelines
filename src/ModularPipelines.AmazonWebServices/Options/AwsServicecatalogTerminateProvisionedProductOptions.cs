using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "terminate-provisioned-product")]
public record AwsServicecatalogTerminateProvisionedProductOptions : AwsOptions
{
    [CliOption("--provisioned-product-name")]
    public string? ProvisionedProductName { get; set; }

    [CliOption("--provisioned-product-id")]
    public string? ProvisionedProductId { get; set; }

    [CliOption("--terminate-token")]
    public string? TerminateToken { get; set; }

    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}