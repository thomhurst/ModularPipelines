using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "get-deployment")]
public record AwsApigatewayv2GetDeploymentOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--deployment-id")] string DeploymentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}