using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "gateway-security-policies", "export")]
public record GcloudNetworkSecurityGatewaySecurityPoliciesExportOptions(
[property: CliArgument] string GatewaySecurityPolicy,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }
}