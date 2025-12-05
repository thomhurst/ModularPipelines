using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "delete-deployment")]
public record AwsApigatewayDeleteDeploymentOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--deployment-id")] string DeploymentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}