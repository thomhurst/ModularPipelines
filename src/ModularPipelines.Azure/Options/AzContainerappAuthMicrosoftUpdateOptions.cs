using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "auth", "microsoft", "update")]
public record AzContainerappAuthMicrosoftUpdateOptions : AzOptions
{
    [BooleanCommandSwitch("--allowed-audiences")]
    public bool? AllowedAudiences { get; set; }

    [CommandSwitch("--certificate-issuer")]
    public string? CertificateIssuer { get; set; }

    [CommandSwitch("--client-id")]
    public string? ClientId { get; set; }

    [CommandSwitch("--client-secret")]
    public string? ClientSecret { get; set; }

    [CommandSwitch("--client-secret-certificate-san")]
    public string? ClientSecretCertificateSan { get; set; }

    [CommandSwitch("--client-secret-certificate-thumbprint")]
    public string? ClientSecretCertificateThumbprint { get; set; }

    [CommandSwitch("--client-secret-name")]
    public string? ClientSecretName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--issuer")]
    public string? Issuer { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--tenant-id")]
    public string? TenantId { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}