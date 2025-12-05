using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "delete-gateway-response")]
public record AwsApigatewayDeleteGatewayResponseOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--response-type")] string ResponseType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}