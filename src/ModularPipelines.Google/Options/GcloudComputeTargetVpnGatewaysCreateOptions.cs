using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-vpn-gateways", "create")]
public record GcloudComputeTargetVpnGatewaysCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--network")] string Network
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}