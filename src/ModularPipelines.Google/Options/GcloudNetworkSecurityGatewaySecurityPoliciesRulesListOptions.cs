using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "gateway-security-policies", "rules", "list")]
public record GcloudNetworkSecurityGatewaySecurityPoliciesRulesListOptions(
[property: CliOption("--gateway-security-policy")] string GatewaySecurityPolicy,
[property: CliOption("--location")] string Location
) : GcloudOptions;