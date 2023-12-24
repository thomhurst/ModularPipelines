using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "disassociate-service-action-from-provisioning-artifact")]
public record AwsServicecatalogDisassociateServiceActionFromProvisioningArtifactOptions(
[property: CommandSwitch("--product-id")] string ProductId,
[property: CommandSwitch("--provisioning-artifact-id")] string ProvisioningArtifactId,
[property: CommandSwitch("--service-action-id")] string ServiceActionId
) : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}