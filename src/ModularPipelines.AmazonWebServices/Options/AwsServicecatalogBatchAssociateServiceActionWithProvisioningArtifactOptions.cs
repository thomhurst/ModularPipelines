using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "batch-associate-service-action-with-provisioning-artifact")]
public record AwsServicecatalogBatchAssociateServiceActionWithProvisioningArtifactOptions(
[property: CommandSwitch("--service-action-associations")] string[] ServiceActionAssociations
) : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}