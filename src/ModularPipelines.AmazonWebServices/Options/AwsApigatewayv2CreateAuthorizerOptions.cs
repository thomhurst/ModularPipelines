using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "create-authorizer")]
public record AwsApigatewayv2CreateAuthorizerOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--authorizer-type")] string AuthorizerType,
[property: CommandSwitch("--identity-source")] string[] IdentitySource,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--authorizer-credentials-arn")]
    public string? AuthorizerCredentialsArn { get; set; }

    [CommandSwitch("--authorizer-payload-format-version")]
    public string? AuthorizerPayloadFormatVersion { get; set; }

    [CommandSwitch("--authorizer-result-ttl-in-seconds")]
    public int? AuthorizerResultTtlInSeconds { get; set; }

    [CommandSwitch("--authorizer-uri")]
    public string? AuthorizerUri { get; set; }

    [CommandSwitch("--identity-validation-expression")]
    public string? IdentityValidationExpression { get; set; }

    [CommandSwitch("--jwt-configuration")]
    public string? JwtConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}