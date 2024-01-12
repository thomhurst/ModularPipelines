using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "client-tls-policies", "export")]
public record GcloudNetworkSecurityClientTlsPoliciesExportOptions(
[property: PositionalArgument] string ClientTlsPolicy,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }
}