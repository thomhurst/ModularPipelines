using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "server-tls-policies", "delete")]
public record GcloudNetworkSecurityServerTlsPoliciesDeleteOptions(
[property: CliArgument] string ServerTlsPolicy,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}