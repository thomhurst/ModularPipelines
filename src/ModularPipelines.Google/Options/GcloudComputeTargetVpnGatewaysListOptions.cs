using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-vpn-gateways", "list")]
public record GcloudComputeTargetVpnGatewaysListOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--regexp")]
    public string? Regexp { get; set; }

    [CommandSwitch("--regions")]
    public string[]? Regions { get; set; }
}