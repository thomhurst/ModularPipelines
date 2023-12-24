using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "update-user-pool-client")]
public record AwsCognitoIdpUpdateUserPoolClientOptions(
[property: CommandSwitch("--user-pool-id")] string UserPoolId,
[property: CommandSwitch("--client-id")] string ClientId
) : AwsOptions
{
    [CommandSwitch("--client-name")]
    public string? ClientName { get; set; }

    [CommandSwitch("--refresh-token-validity")]
    public int? RefreshTokenValidity { get; set; }

    [CommandSwitch("--access-token-validity")]
    public int? AccessTokenValidity { get; set; }

    [CommandSwitch("--id-token-validity")]
    public int? IdTokenValidity { get; set; }

    [CommandSwitch("--token-validity-units")]
    public string? TokenValidityUnits { get; set; }

    [CommandSwitch("--read-attributes")]
    public string[]? ReadAttributes { get; set; }

    [CommandSwitch("--write-attributes")]
    public string[]? WriteAttributes { get; set; }

    [CommandSwitch("--explicit-auth-flows")]
    public string[]? ExplicitAuthFlows { get; set; }

    [CommandSwitch("--supported-identity-providers")]
    public string[]? SupportedIdentityProviders { get; set; }

    [CommandSwitch("--callback-urls")]
    public string[]? CallbackUrls { get; set; }

    [CommandSwitch("--logout-urls")]
    public string[]? LogoutUrls { get; set; }

    [CommandSwitch("--default-redirect-uri")]
    public string? DefaultRedirectUri { get; set; }

    [CommandSwitch("--allowed-o-auth-flows")]
    public string[]? AllowedOAuthFlows { get; set; }

    [CommandSwitch("--allowed-o-auth-scopes")]
    public string[]? AllowedOAuthScopes { get; set; }

    [CommandSwitch("--analytics-configuration")]
    public string? AnalyticsConfiguration { get; set; }

    [CommandSwitch("--prevent-user-existence-errors")]
    public string? PreventUserExistenceErrors { get; set; }

    [CommandSwitch("--auth-session-validity")]
    public int? AuthSessionValidity { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}