using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "execute-provisioned-product-service-action")]
public record AwsServicecatalogExecuteProvisionedProductServiceActionOptions(
[property: CliOption("--provisioned-product-id")] string ProvisionedProductId,
[property: CliOption("--service-action-id")] string ServiceActionId
) : AwsOptions
{
    [CliOption("--execute-token")]
    public string? ExecuteToken { get; set; }

    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}