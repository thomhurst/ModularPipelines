using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "terminate-provisioned-product")]
public record AwsServicecatalogTerminateProvisionedProductOptions : AwsOptions
{
    [CommandSwitch("--provisioned-product-name")]
    public string? ProvisionedProductName { get; set; }

    [CommandSwitch("--provisioned-product-id")]
    public string? ProvisionedProductId { get; set; }

    [CommandSwitch("--terminate-token")]
    public string? TerminateToken { get; set; }

    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}