using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "dns-authorizations", "delete")]
public record GcloudCertificateManagerDnsAuthorizationsDeleteOptions(
[property: CliArgument] string DnsAuthorization,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}