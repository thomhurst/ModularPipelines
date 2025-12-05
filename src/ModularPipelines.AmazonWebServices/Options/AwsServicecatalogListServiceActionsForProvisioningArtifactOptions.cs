using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "list-service-actions-for-provisioning-artifact")]
public record AwsServicecatalogListServiceActionsForProvisioningArtifactOptions(
[property: CliOption("--product-id")] string ProductId,
[property: CliOption("--provisioning-artifact-id")] string ProvisioningArtifactId
) : AwsOptions
{
    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}