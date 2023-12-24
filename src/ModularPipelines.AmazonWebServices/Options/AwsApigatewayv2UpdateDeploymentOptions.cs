using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "update-deployment")]
public record AwsApigatewayv2UpdateDeploymentOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--deployment-id")] string DeploymentId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}