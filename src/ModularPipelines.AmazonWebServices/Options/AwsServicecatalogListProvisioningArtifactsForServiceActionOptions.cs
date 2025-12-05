using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "list-provisioning-artifacts-for-service-action")]
public record AwsServicecatalogListProvisioningArtifactsForServiceActionOptions(
[property: CliOption("--service-action-id")] string ServiceActionId
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