using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "get-integration-response")]
public record AwsApigatewayv2GetIntegrationResponseOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--integration-id")] string IntegrationId,
[property: CliOption("--integration-response-id")] string IntegrationResponseId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}