using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-identity", "create-identity-pool")]
public record AwsCognitoIdentityCreateIdentityPoolOptions(
[property: CliOption("--identity-pool-name")] string IdentityPoolName
) : AwsOptions
{
    [CliOption("--supported-login-providers")]
    public IEnumerable<KeyValue>? SupportedLoginProviders { get; set; }

    [CliOption("--developer-provider-name")]
    public string? DeveloperProviderName { get; set; }

    [CliOption("--open-id-connect-provider-arns")]
    public string[]? OpenIdConnectProviderArns { get; set; }

    [CliOption("--cognito-identity-providers")]
    public string[]? CognitoIdentityProviders { get; set; }

    [CliOption("--saml-provider-arns")]
    public string[]? SamlProviderArns { get; set; }

    [CliOption("--identity-pool-tags")]
    public IEnumerable<KeyValue>? IdentityPoolTags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}