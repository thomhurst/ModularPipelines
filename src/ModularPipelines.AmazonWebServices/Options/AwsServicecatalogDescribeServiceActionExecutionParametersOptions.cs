using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "describe-service-action-execution-parameters")]
public record AwsServicecatalogDescribeServiceActionExecutionParametersOptions(
[property: CliOption("--provisioned-product-id")] string ProvisionedProductId,
[property: CliOption("--service-action-id")] string ServiceActionId
) : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}