using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "get-integration")]
public record AwsApigatewayv2GetIntegrationOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--integration-id")] string IntegrationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}