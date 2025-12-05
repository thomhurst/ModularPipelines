using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-vpn-gateways", "delete")]
public record GcloudComputeTargetVpnGatewaysDeleteOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}