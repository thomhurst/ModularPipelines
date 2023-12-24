using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "create-authorizer")]
public record AwsApigatewayCreateAuthorizerOptions(
[property: CommandSwitch("--rest-api-id")] string RestApiId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--provider-arns")]
    public string[]? ProviderArns { get; set; }

    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--authorizer-uri")]
    public string? AuthorizerUri { get; set; }

    [CommandSwitch("--authorizer-credentials")]
    public string? AuthorizerCredentials { get; set; }

    [CommandSwitch("--identity-source")]
    public string? IdentitySource { get; set; }

    [CommandSwitch("--identity-validation-expression")]
    public string? IdentityValidationExpression { get; set; }

    [CommandSwitch("--authorizer-result-ttl-in-seconds")]
    public int? AuthorizerResultTtlInSeconds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}