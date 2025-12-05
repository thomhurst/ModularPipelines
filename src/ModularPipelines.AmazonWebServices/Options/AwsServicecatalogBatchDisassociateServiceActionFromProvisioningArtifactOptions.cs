using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "batch-disassociate-service-action-from-provisioning-artifact")]
public record AwsServicecatalogBatchDisassociateServiceActionFromProvisioningArtifactOptions(
[property: CliOption("--service-action-associations")] string[] ServiceActionAssociations
) : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}