using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "gateway-security-policies", "rules", "delete")]
public record GcloudNetworkSecurityGatewaySecurityPoliciesRulesDeleteOptions(
[property: PositionalArgument] string GatewaySecurityPolicyRule,
[property: PositionalArgument] string GatewaySecurityPolicy,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}