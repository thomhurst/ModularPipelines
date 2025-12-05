using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "get-deployment")]
public record AwsApigatewayGetDeploymentOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--deployment-id")] string DeploymentId
) : AwsOptions
{
    [CliOption("--embed")]
    public string[]? Embed { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}