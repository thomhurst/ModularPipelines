using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "get-method-response")]
public record AwsApigatewayGetMethodResponseOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--http-method")] string HttpMethod,
[property: CliOption("--status-code")] string StatusCode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}