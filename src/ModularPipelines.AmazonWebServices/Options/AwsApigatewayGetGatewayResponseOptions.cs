using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "get-gateway-response")]
public record AwsApigatewayGetGatewayResponseOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--response-type")] string ResponseType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}