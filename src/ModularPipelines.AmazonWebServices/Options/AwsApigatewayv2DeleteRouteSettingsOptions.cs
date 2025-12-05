using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "delete-route-settings")]
public record AwsApigatewayv2DeleteRouteSettingsOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--route-key")] string RouteKey,
[property: CliOption("--stage-name")] string StageName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}