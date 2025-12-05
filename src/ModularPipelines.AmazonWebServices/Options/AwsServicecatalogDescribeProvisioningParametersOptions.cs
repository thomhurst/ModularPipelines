using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "describe-provisioning-parameters")]
public record AwsServicecatalogDescribeProvisioningParametersOptions : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

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

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}