using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "vpn-tunnels", "describe")]
public record GcloudComputeVpnTunnelsDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}