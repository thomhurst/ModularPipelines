using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "update-provisioned-product")]
public record AwsServicecatalogUpdateProvisionedProductOptions : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--provisioned-product-name")]
    public string? ProvisionedProductName { get; set; }

    [CliOption("--provisioned-product-id")]
    public string? ProvisionedProductId { get; set; }

    [CliOption("--product-id")]
    public string? ProductId { get; set; }

    [CliOption("--product-name")]
    public string? ProductName { get; set; }

    [CliOption("--provisioning-artifact-id")]
    public string? ProvisioningArtifactId { get; set; }

    [CliOption("--provisioning-artifact-name")]
    public string? ProvisioningArtifactName { get; set; }

    [CliOption("--path-id")]
    public string? PathId { get; set; }

    [CliOption("--path-name")]
    public string? PathName { get; set; }

    [CliOption("--provisioning-parameters")]
    public string[]? ProvisioningParameters { get; set; }

    [CliOption("--provisioning-preferences")]
    public string? ProvisioningPreferences { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--update-token")]
    public string? UpdateToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}