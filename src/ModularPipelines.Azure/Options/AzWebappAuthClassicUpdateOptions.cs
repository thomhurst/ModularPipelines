using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "auth-classic", "update")]
public record AzWebappAuthClassicUpdateOptions : AzOptions
{
    [CommandSwitch("--aad-allowed-token-audiences")]
    public string? AadAllowedTokenAudiences { get; set; }

    [CommandSwitch("--aad-client-id")]
    public string? AadClientId { get; set; }

    [CommandSwitch("--aad-client-secret")]
    public string? AadClientSecret { get; set; }

    [CommandSwitch("--aad-client-secret-certificate-thumbprint")]
    public string? AadClientSecretCertificateThumbprint { get; set; }

    [CommandSwitch("--aad-client-secret-setting-name")]
    public string? AadClientSecretSettingName { get; set; }

    [CommandSwitch("--aad-token-issuer-url")]
    public string? AadTokenIssuerUrl { get; set; }

    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [BooleanCommandSwitch("--allowed-redirect-urls")]
    public bool? AllowedRedirectUrls { get; set; }

    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--facebook-app-id")]
    public string? FacebookAppId { get; set; }

    [CommandSwitch("--facebook-app-secret")]
    public string? FacebookAppSecret { get; set; }

    [CommandSwitch("--facebook-app-secret-setting-name")]
    public string? FacebookAppSecretSettingName { get; set; }

    [CommandSwitch("--facebook-oauth-scopes")]
    public string? FacebookOauthScopes { get; set; }

    [CommandSwitch("--github-client-id")]
    public string? GithubClientId { get; set; }

    [CommandSwitch("--github-client-secret")]
    public string? GithubClientSecret { get; set; }

    [CommandSwitch("--github-client-secret-setting-name")]
    public string? GithubClientSecretSettingName { get; set; }

    [CommandSwitch("--github-oauth-scopes")]
    public string? GithubOauthScopes { get; set; }

    [CommandSwitch("--google-client-id")]
    public string? GoogleClientId { get; set; }

    [CommandSwitch("--google-client-secret")]
    public string? GoogleClientSecret { get; set; }

    [CommandSwitch("--google-client-secret-setting-name")]
    public string? GoogleClientSecretSettingName { get; set; }

    [CommandSwitch("--google-oauth-scopes")]
    public string? GoogleOauthScopes { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--microsoft-account-client-id")]
    public int? MicrosoftAccountClientId { get; set; }

    [CommandSwitch("--microsoft-account-client-secret")]
    public int? MicrosoftAccountClientSecret { get; set; }

    [CommandSwitch("--microsoft-account-client-secret-setting-name")]
    public int? MicrosoftAccountClientSecretSettingName { get; set; }

    [CommandSwitch("--microsoft-account-oauth-scopes")]
    public int? MicrosoftAccountOauthScopes { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--token-refresh-extension-hours")]
    public string? TokenRefreshExtensionHours { get; set; }

    [BooleanCommandSwitch("--token-store")]
    public bool? TokenStore { get; set; }

    [CommandSwitch("--twitter-consumer-key")]
    public string? TwitterConsumerKey { get; set; }

    [CommandSwitch("--twitter-consumer-secret")]
    public string? TwitterConsumerSecret { get; set; }

    [CommandSwitch("--twitter-consumer-secret-setting-name")]
    public string? TwitterConsumerSecretSettingName { get; set; }
}