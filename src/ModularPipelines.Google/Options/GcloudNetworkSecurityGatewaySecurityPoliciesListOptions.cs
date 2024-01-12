using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "gateway-security-policies", "list")]
public record GcloudNetworkSecurityGatewaySecurityPoliciesListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;