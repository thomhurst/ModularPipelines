using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "auth", "microsoft", "update")]
public record AzContainerappAuthMicrosoftUpdateOptions : AzOptions
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

    [CliOption("--client-secret-name")]
    public string? ClientSecretName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--issuer")]
    public string? Issuer { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tenant-id")]
    public string? TenantId { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}