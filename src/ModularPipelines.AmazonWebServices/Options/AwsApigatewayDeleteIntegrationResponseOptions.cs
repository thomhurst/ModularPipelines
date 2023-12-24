using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "delete-integration-response")]
public record AwsApigatewayDeleteIntegrationResponseOptions(
[property: CommandSwitch("--rest-api-id")] string RestApiId,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--http-method")] string HttpMethod,
[property: CommandSwitch("--status-code")] string StatusCode
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}