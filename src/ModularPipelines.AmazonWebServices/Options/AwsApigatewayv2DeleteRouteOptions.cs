using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "delete-route")]
public record AwsApigatewayv2DeleteRouteOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--route-id")] string RouteId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}