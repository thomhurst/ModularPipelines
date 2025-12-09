using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "associate-service-action-with-provisioning-artifact")]
public record AwsServicecatalogAssociateServiceActionWithProvisioningArtifactOptions(
[property: CliOption("--product-id")] string ProductId,
[property: CliOption("--provisioning-artifact-id")] string ProvisioningArtifactId,
[property: CliOption("--service-action-id")] string ServiceActionId
) : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}