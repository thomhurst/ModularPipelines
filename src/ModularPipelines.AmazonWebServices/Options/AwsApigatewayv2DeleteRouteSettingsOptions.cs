using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "delete-route-settings")]
public record AwsApigatewayv2DeleteRouteSettingsOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--route-key")] string RouteKey,
[property: CommandSwitch("--stage-name")] string StageName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}