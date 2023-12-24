using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "delete-provisioning-artifact")]
public record AwsServicecatalogDeleteProvisioningArtifactOptions(
[property: CommandSwitch("--product-id")] string ProductId,
[property: CommandSwitch("--provisioning-artifact-id")] string ProvisioningArtifactId
) : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}