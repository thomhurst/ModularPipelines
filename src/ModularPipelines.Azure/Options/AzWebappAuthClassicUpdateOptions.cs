using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "auth-classic", "update")]
public record AzWebappAuthClassicUpdateOptions : AzOptions
{
    [CliOption("--aad-allowed-token-audiences")]
    public string? AadAllowedTokenAudiences { get; set; }

    [CliOption("--aad-client-id")]
    public string? AadClientId { get; set; }

    [CliOption("--aad-client-secret")]
    public string? AadClientSecret { get; set; }

    [CliOption("--aad-client-secret-certificate-thumbprint")]
    public string? AadClientSecretCertificateThumbprint { get; set; }

    [CliOption("--aad-client-secret-setting-name")]
    public string? AadClientSecretSettingName { get; set; }

    [CliOption("--aad-token-issuer-url")]
    public string? AadTokenIssuerUrl { get; set; }

    [CliOption("--action")]
    public string? Action { get; set; }

    [CliFlag("--allowed-redirect-urls")]
    public bool? AllowedRedirectUrls { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--facebook-app-id")]
    public string? FacebookAppId { get; set; }

    [CliOption("--facebook-app-secret")]
    public string? FacebookAppSecret { get; set; }

    [CliOption("--facebook-app-secret-setting-name")]
    public string? FacebookAppSecretSettingName { get; set; }

    [CliOption("--facebook-oauth-scopes")]
    public string? FacebookOauthScopes { get; set; }

    [CliOption("--github-client-id")]
    public string? GithubClientId { get; set; }

    [CliOption("--github-client-secret")]
    public string? GithubClientSecret { get; set; }

    [CliOption("--github-client-secret-setting-name")]
    public string? GithubClientSecretSettingName { get; set; }

    [CliOption("--github-oauth-scopes")]
    public string? GithubOauthScopes { get; set; }

    [CliOption("--google-client-id")]
    public string? GoogleClientId { get; set; }

    [CliOption("--google-client-secret")]
    public string? GoogleClientSecret { get; set; }

    [CliOption("--google-client-secret-setting-name")]
    public string? GoogleClientSecretSettingName { get; set; }

    [CliOption("--google-oauth-scopes")]
    public string? GoogleOauthScopes { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--microsoft-account-client-id")]
    public int? MicrosoftAccountClientId { get; set; }

    [CliOption("--microsoft-account-client-secret")]
    public int? MicrosoftAccountClientSecret { get; set; }

    [CliOption("--microsoft-account-client-secret-setting-name")]
    public int? MicrosoftAccountClientSecretSettingName { get; set; }

    [CliOption("--microsoft-account-oauth-scopes")]
    public int? MicrosoftAccountOauthScopes { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--token-refresh-extension-hours")]
    public string? TokenRefreshExtensionHours { get; set; }

    [CliFlag("--token-store")]
    public bool? TokenStore { get; set; }

    [CliOption("--twitter-consumer-key")]
    public string? TwitterConsumerKey { get; set; }

    [CliOption("--twitter-consumer-secret")]
    public string? TwitterConsumerSecret { get; set; }

    [CliOption("--twitter-consumer-secret-setting-name")]
    public string? TwitterConsumerSecretSettingName { get; set; }
}