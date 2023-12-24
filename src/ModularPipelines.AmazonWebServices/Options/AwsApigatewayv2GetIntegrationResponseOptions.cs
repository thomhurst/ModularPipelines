using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "get-integration-response")]
public record AwsApigatewayv2GetIntegrationResponseOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--integration-id")] string IntegrationId,
[property: CommandSwitch("--integration-response-id")] string IntegrationResponseId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}