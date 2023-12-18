using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "custom-domain", "create")]
public record AzAfdCustomDomainCreateOptions(
[property: CommandSwitch("--certificate-type")] string CertificateType,
[property: CommandSwitch("--custom-domain-name")] string CustomDomainName,
[property: CommandSwitch("--host-name")] string HostName,
[property: CommandSwitch("--minimum-tls-version")] string MinimumTlsVersion,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--azure-dns-zone")]
    public string? AzureDnsZone { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--secret")]
    public string? Secret { get; set; }
}