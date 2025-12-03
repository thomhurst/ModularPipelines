using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "gateway-security-policies", "rules", "export")]
public record GcloudNetworkSecurityGatewaySecurityPoliciesRulesExportOptions(
[property: CliArgument] string GatewaySecurityPolicyRule,
[property: CliArgument] string GatewaySecurityPolicy,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }
}