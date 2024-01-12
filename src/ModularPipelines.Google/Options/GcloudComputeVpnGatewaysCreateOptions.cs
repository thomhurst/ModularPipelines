using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "vpn-gateways", "create")]
public record GcloudComputeVpnGatewaysCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--network")] string Network
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--interconnect-attachments")]
    public string[]? InterconnectAttachments { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--stack-type")]
    public string? StackType { get; set; }
}