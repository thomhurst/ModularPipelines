using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "delete-integration")]
public record AwsApigatewayv2DeleteIntegrationOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--integration-id")] string IntegrationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}