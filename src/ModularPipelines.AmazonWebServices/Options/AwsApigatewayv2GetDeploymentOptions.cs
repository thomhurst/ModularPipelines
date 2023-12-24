using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "get-deployment")]
public record AwsApigatewayv2GetDeploymentOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--deployment-id")] string DeploymentId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}