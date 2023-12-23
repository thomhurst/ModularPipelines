using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "describe-provisioning-parameters")]
public record AwsServicecatalogDescribeProvisioningParametersOptions : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

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

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}