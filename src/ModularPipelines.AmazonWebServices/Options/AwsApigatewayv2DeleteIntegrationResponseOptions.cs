using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "delete-integration-response")]
public record AwsApigatewayv2DeleteIntegrationResponseOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--integration-id")] string IntegrationId,
[property: CliOption("--integration-response-id")] string IntegrationResponseId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}