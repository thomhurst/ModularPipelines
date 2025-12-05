using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "create-authorizer")]
public record AwsApigatewayCreateAuthorizerOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--name")] string Name,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--provider-arns")]
    public string[]? ProviderArns { get; set; }

    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--authorizer-uri")]
    public string? AuthorizerUri { get; set; }

    [CliOption("--authorizer-credentials")]
    public string? AuthorizerCredentials { get; set; }

    [CliOption("--identity-source")]
    public string? IdentitySource { get; set; }

    [CliOption("--identity-validation-expression")]
    public string? IdentityValidationExpression { get; set; }

    [CliOption("--authorizer-result-ttl-in-seconds")]
    public int? AuthorizerResultTtlInSeconds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}