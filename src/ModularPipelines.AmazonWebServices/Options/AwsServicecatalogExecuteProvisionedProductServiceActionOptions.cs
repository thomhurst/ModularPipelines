using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "execute-provisioned-product-service-action")]
public record AwsServicecatalogExecuteProvisionedProductServiceActionOptions(
[property: CommandSwitch("--provisioned-product-id")] string ProvisionedProductId,
[property: CommandSwitch("--service-action-id")] string ServiceActionId
) : AwsOptions
{
    [CommandSwitch("--execute-token")]
    public string? ExecuteToken { get; set; }

    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}