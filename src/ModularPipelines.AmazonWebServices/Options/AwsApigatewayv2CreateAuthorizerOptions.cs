using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "create-authorizer")]
public record AwsApigatewayv2CreateAuthorizerOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--authorizer-type")] string AuthorizerType,
[property: CliOption("--identity-source")] string[] IdentitySource,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--authorizer-credentials-arn")]
    public string? AuthorizerCredentialsArn { get; set; }

    [CliOption("--authorizer-payload-format-version")]
    public string? AuthorizerPayloadFormatVersion { get; set; }

    [CliOption("--authorizer-result-ttl-in-seconds")]
    public int? AuthorizerResultTtlInSeconds { get; set; }

    [CliOption("--authorizer-uri")]
    public string? AuthorizerUri { get; set; }

    [CliOption("--identity-validation-expression")]
    public string? IdentityValidationExpression { get; set; }

    [CliOption("--jwt-configuration")]
    public string? JwtConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}