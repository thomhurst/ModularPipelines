using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "server-tls-policies", "export")]
public record GcloudNetworkSecurityServerTlsPoliciesExportOptions(
[property: CliArgument] string ServerTlsPolicy,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }
}