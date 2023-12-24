using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "update-authorizer")]
public record AwsApigatewayv2UpdateAuthorizerOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--authorizer-id")] string AuthorizerId
) : AwsOptions
{
    [CommandSwitch("--authorizer-credentials-arn")]
    public string? AuthorizerCredentialsArn { get; set; }

    [CommandSwitch("--authorizer-payload-format-version")]
    public string? AuthorizerPayloadFormatVersion { get; set; }

    [CommandSwitch("--authorizer-result-ttl-in-seconds")]
    public int? AuthorizerResultTtlInSeconds { get; set; }

    [CommandSwitch("--authorizer-type")]
    public string? AuthorizerType { get; set; }

    [CommandSwitch("--authorizer-uri")]
    public string? AuthorizerUri { get; set; }

    [CommandSwitch("--identity-source")]
    public string[]? IdentitySource { get; set; }

    [CommandSwitch("--identity-validation-expression")]
    public string? IdentityValidationExpression { get; set; }

    [CommandSwitch("--jwt-configuration")]
    public string? JwtConfiguration { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}