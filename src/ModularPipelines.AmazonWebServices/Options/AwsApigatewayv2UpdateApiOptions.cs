using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "update-api")]
public record AwsApigatewayv2UpdateApiOptions(
[property: CommandSwitch("--api-id")] string ApiId
) : AwsOptions
{
    [CommandSwitch("--api-key-selection-expression")]
    public string? ApiKeySelectionExpression { get; set; }

    [CommandSwitch("--cors-configuration")]
    public string? CorsConfiguration { get; set; }

    [CommandSwitch("--credentials-arn")]
    public string? CredentialsArn { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--route-key")]
    public string? RouteKey { get; set; }

    [CommandSwitch("--route-selection-expression")]
    public string? RouteSelectionExpression { get; set; }

    [CommandSwitch("--target")]
    public string? Target { get; set; }

    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}