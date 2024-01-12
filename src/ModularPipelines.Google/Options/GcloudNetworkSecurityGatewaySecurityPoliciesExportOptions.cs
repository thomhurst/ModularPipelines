using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "gateway-security-policies", "export")]
public record GcloudNetworkSecurityGatewaySecurityPoliciesExportOptions(
[property: PositionalArgument] string GatewaySecurityPolicy,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }
}