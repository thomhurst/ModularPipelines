using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "auth", "update")]
public record AzWebappAuthUpdateOptions : AzOptions
{
    [CommandSwitch("--aad-allowed-token-audiences")]
    public string? AadAllowedTokenAudiences { get; set; }

    [CommandSwitch("--aad-client-id")]
    public string? AadClientId { get; set; }

    [CommandSwitch("--aad-client-secret")]
    public string? AadClientSecret { get; set; }

    [CommandSwitch("--aad-client-secret-certificate-thumbprint")]
    public string? AadClientSecretCertificateThumbprint { get; set; }

    [CommandSwitch("--aad-token-issuer-url")]
    public string? AadTokenIssuerUrl { get; set; }

    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [BooleanCommandSwitch("--allowed-external-redirect-urls")]
    public bool? AllowedExternalRedirectUrls { get; set; }

    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--facebook-app-id")]
    public string? FacebookAppId { get; set; }

    [CommandSwitch("--facebook-app-secret")]
    public string? FacebookAppSecret { get; set; }

    [CommandSwitch("--facebook-oauth-scopes")]
    public string? FacebookOauthScopes { get; set; }

    [CommandSwitch("--google-client-id")]
    public string? GoogleClientId { get; set; }

    [CommandSwitch("--google-client-secret")]
    public string? GoogleClientSecret { get; set; }

    [CommandSwitch("--google-oauth-scopes")]
    public string? GoogleOauthScopes { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--microsoft-account-client-id")]
    public int? MicrosoftAccountClientId { get; set; }

    [CommandSwitch("--microsoft-account-client-secret")]
    public int? MicrosoftAccountClientSecret { get; set; }

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
}