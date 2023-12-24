using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "batch-disassociate-service-action-from-provisioning-artifact")]
public record AwsServicecatalogBatchDisassociateServiceActionFromProvisioningArtifactOptions(
[property: CommandSwitch("--service-action-associations")] string[] ServiceActionAssociations
) : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}