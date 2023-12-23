using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-identity", "update-identity-pool")]
public record AwsCognitoIdentityUpdateIdentityPoolOptions(
[property: CommandSwitch("--identity-pool-id")] string IdentityPoolId,
[property: CommandSwitch("--identity-pool-name")] string IdentityPoolName
) : AwsOptions
{
    [CommandSwitch("--supported-login-providers")]
    public IEnumerable<KeyValue>? SupportedLoginProviders { get; set; }

    [CommandSwitch("--developer-provider-name")]
    public string? DeveloperProviderName { get; set; }

    [CommandSwitch("--open-id-connect-provider-arns")]
    public string[]? OpenIdConnectProviderArns { get; set; }

    [CommandSwitch("--cognito-identity-providers")]
    public string[]? CognitoIdentityProviders { get; set; }

    [CommandSwitch("--saml-provider-arns")]
    public string[]? SamlProviderArns { get; set; }

    [CommandSwitch("--identity-pool-tags")]
    public IEnumerable<KeyValue>? IdentityPoolTags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}