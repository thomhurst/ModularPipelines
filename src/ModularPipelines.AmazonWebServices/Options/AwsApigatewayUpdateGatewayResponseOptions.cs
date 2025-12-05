using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "update-gateway-response")]
public record AwsApigatewayUpdateGatewayResponseOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--response-type")] string ResponseType
) : AwsOptions
{
    [CliOption("--patch-operations")]
    public string[]? PatchOperations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}