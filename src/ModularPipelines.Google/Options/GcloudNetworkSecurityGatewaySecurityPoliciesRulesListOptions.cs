using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "gateway-security-policies", "rules", "list")]
public record GcloudNetworkSecurityGatewaySecurityPoliciesRulesListOptions(
[property: CommandSwitch("--gateway-security-policy")] string GatewaySecurityPolicy,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;