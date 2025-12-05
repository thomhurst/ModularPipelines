using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "gateway-security-policies", "delete")]
public record GcloudNetworkSecurityGatewaySecurityPoliciesDeleteOptions(
[property: CliArgument] string GatewaySecurityPolicy,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}