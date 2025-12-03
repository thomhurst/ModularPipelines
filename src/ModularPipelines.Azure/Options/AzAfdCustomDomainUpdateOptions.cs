using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("afd", "custom-domain", "update")]
public record AzAfdCustomDomainUpdateOptions : AzOptions
{
    [CliOption("--azure-dns-zone")]
    public string? AzureDnsZone { get; set; }

    [CliOption("--certificate-type")]
    public string? CertificateType { get; set; }

    [CliOption("--custom-domain-name")]
    public string? CustomDomainName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--minimum-tls-version")]
    public string? MinimumTlsVersion { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--secret")]
    public string? Secret { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}