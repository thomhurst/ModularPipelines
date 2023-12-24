using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "create-api")]
public record AwsApigatewayv2CreateApiOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--protocol-type")] string ProtocolType
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

    [CommandSwitch("--route-key")]
    public string? RouteKey { get; set; }

    [CommandSwitch("--route-selection-expression")]
    public string? RouteSelectionExpression { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--target")]
    public string? Target { get; set; }

    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}