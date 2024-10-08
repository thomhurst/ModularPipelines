using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "gateway-security-policies", "rules", "import")]
public record GcloudNetworkSecurityGatewaySecurityPoliciesRulesImportOptions(
[property: PositionalArgument] string GatewaySecurityPolicyRule,
[property: PositionalArgument] string GatewaySecurityPolicy,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }
}