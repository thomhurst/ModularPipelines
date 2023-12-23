using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "describe-service-action-execution-parameters")]
public record AwsServicecatalogDescribeServiceActionExecutionParametersOptions(
[property: CommandSwitch("--provisioned-product-id")] string ProvisionedProductId,
[property: CommandSwitch("--service-action-id")] string ServiceActionId
) : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}