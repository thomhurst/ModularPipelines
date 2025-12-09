using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "delete-route")]
public record AwsApigatewayv2DeleteRouteOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--route-id")] string RouteId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}