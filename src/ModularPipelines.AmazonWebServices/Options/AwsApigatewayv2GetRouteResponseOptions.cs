using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "get-route-response")]
public record AwsApigatewayv2GetRouteResponseOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--route-id")] string RouteId,
[property: CliOption("--route-response-id")] string RouteResponseId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}