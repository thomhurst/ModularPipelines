using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "server-tls-policies", "export")]
public record GcloudNetworkSecurityServerTlsPoliciesExportOptions(
[property: PositionalArgument] string ServerTlsPolicy,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }
}