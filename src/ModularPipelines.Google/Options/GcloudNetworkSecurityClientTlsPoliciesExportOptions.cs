using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "client-tls-policies", "export")]
public record GcloudNetworkSecurityClientTlsPoliciesExportOptions(
[property: CliArgument] string ClientTlsPolicy,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }
}