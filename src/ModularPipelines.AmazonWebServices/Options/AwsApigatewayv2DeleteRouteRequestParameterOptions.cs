using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "delete-route-request-parameter")]
public record AwsApigatewayv2DeleteRouteRequestParameterOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--request-parameter-key")] string RequestParameterKey,
[property: CliOption("--route-id")] string RouteId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}