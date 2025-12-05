using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "client-tls-policies", "delete")]
public record GcloudNetworkSecurityClientTlsPoliciesDeleteOptions(
[property: CliArgument] string ClientTlsPolicy,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}