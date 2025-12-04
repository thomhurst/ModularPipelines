using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd", "custom-domain", "create")]
public record AzAfdCustomDomainCreateOptions(
[property: CliOption("--certificate-type")] string CertificateType,
[property: CliOption("--custom-domain-name")] string CustomDomainName,
[property: CliOption("--host-name")] string HostName,
[property: CliOption("--minimum-tls-version")] string MinimumTlsVersion,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--azure-dns-zone")]
    public string? AzureDnsZone { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--secret")]
    public string? Secret { get; set; }
}