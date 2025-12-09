using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "update-user-pool-client")]
public record AwsCognitoIdpUpdateUserPoolClientOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--client-id")] string ClientId
) : AwsOptions
{
    [CliOption("--client-name")]
    public string? ClientName { get; set; }

    [CliOption("--refresh-token-validity")]
    public int? RefreshTokenValidity { get; set; }

    [CliOption("--access-token-validity")]
    public int? AccessTokenValidity { get; set; }

    [CliOption("--id-token-validity")]
    public int? IdTokenValidity { get; set; }

    [CliOption("--token-validity-units")]
    public string? TokenValidityUnits { get; set; }

    [CliOption("--read-attributes")]
    public string[]? ReadAttributes { get; set; }

    [CliOption("--write-attributes")]
    public string[]? WriteAttributes { get; set; }

    [CliOption("--explicit-auth-flows")]
    public string[]? ExplicitAuthFlows { get; set; }

    [CliOption("--supported-identity-providers")]
    public string[]? SupportedIdentityProviders { get; set; }

    [CliOption("--callback-urls")]
    public string[]? CallbackUrls { get; set; }

    [CliOption("--logout-urls")]
    public string[]? LogoutUrls { get; set; }

    [CliOption("--default-redirect-uri")]
    public string? DefaultRedirectUri { get; set; }

    [CliOption("--allowed-o-auth-flows")]
    public string[]? AllowedOAuthFlows { get; set; }

    [CliOption("--allowed-o-auth-scopes")]
    public string[]? AllowedOAuthScopes { get; set; }

    [CliOption("--analytics-configuration")]
    public string? AnalyticsConfiguration { get; set; }

    [CliOption("--prevent-user-existence-errors")]
    public string? PreventUserExistenceErrors { get; set; }

    [CliOption("--auth-session-validity")]
    public int? AuthSessionValidity { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}