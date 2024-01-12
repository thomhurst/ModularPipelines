using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-vpn-gateways", "create")]
public record GcloudComputeTargetVpnGatewaysCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--network")] string Network
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}