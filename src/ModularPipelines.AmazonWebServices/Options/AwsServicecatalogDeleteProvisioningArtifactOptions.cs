using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "delete-provisioning-artifact")]
public record AwsServicecatalogDeleteProvisioningArtifactOptions(
[property: CliOption("--product-id")] string ProductId,
[property: CliOption("--provisioning-artifact-id")] string ProvisioningArtifactId
) : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}