using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "gateway-security-policies", "list")]
public record GcloudNetworkSecurityGatewaySecurityPoliciesListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;