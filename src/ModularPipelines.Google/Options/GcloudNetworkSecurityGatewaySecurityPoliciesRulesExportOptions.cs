using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "gateway-security-policies", "rules", "export")]
public record GcloudNetworkSecurityGatewaySecurityPoliciesRulesExportOptions(
[property: PositionalArgument] string GatewaySecurityPolicyRule,
[property: PositionalArgument] string GatewaySecurityPolicy,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }
}