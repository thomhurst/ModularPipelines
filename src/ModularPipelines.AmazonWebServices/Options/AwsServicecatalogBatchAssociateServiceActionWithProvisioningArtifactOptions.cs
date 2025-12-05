using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "batch-associate-service-action-with-provisioning-artifact")]
public record AwsServicecatalogBatchAssociateServiceActionWithProvisioningArtifactOptions(
[property: CliOption("--service-action-associations")] string[] ServiceActionAssociations
) : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}