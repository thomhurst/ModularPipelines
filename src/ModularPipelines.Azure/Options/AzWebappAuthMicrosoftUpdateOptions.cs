using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp", "auth", "microsoft", "update")]
public record AzWebappAuthMicrosoftUpdateOptions : AzOptions
{
    [CliFlag("--allowed-audiences")]
    public bool? AllowedAudiences { get; set; }

    [CliOption("--certificate-issuer")]
    public string? CertificateIssuer { get; set; }

    [CliOption("--client-id")]
    public string? ClientId { get; set; }

    [CliOption("--client-secret")]
    public string? ClientSecret { get; set; }

    [CliOption("--client-secret-certificate-san")]
    public string? ClientSecretCertificateSan { get; set; }

    [CliOption("--client-secret-certificate-thumbprint")]
    public string? ClientSecretCertificateThumbprint { get; set; }

    [CliOption("--client-secret-setting-name")]
    public string? ClientSecretSettingName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--issuer")]
    public string? Issuer { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tenant-id")]
    public string? TenantId { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}