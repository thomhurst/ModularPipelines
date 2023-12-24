using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "update-provisioned-product")]
public record AwsServicecatalogUpdateProvisionedProductOptions : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--provisioned-product-name")]
    public string? ProvisionedProductName { get; set; }

    [CommandSwitch("--provisioned-product-id")]
    public string? ProvisionedProductId { get; set; }

    [CommandSwitch("--product-id")]
    public string? ProductId { get; set; }

    [CommandSwitch("--product-name")]
    public string? ProductName { get; set; }

    [CommandSwitch("--provisioning-artifact-id")]
    public string? ProvisioningArtifactId { get; set; }

    [CommandSwitch("--provisioning-artifact-name")]
    public string? ProvisioningArtifactName { get; set; }

    [CommandSwitch("--path-id")]
    public string? PathId { get; set; }

    [CommandSwitch("--path-name")]
    public string? PathName { get; set; }

    [CommandSwitch("--provisioning-parameters")]
    public string[]? ProvisioningParameters { get; set; }

    [CommandSwitch("--provisioning-preferences")]
    public string? ProvisioningPreferences { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--update-token")]
    public string? UpdateToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}