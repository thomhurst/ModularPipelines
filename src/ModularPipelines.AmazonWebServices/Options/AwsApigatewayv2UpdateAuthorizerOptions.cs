using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "update-authorizer")]
public record AwsApigatewayv2UpdateAuthorizerOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--authorizer-id")] string AuthorizerId
) : AwsOptions
{
    [CliOption("--authorizer-credentials-arn")]
    public string? AuthorizerCredentialsArn { get; set; }

    [CliOption("--authorizer-payload-format-version")]
    public string? AuthorizerPayloadFormatVersion { get; set; }

    [CliOption("--authorizer-result-ttl-in-seconds")]
    public int? AuthorizerResultTtlInSeconds { get; set; }

    [CliOption("--authorizer-type")]
    public string? AuthorizerType { get; set; }

    [CliOption("--authorizer-uri")]
    public string? AuthorizerUri { get; set; }

    [CliOption("--identity-source")]
    public string[]? IdentitySource { get; set; }

    [CliOption("--identity-validation-expression")]
    public string? IdentityValidationExpression { get; set; }

    [CliOption("--jwt-configuration")]
    public string? JwtConfiguration { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}