using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "perimeter", "link", "create")]
public record AzNetworkPerimeterLinkCreateOptions(
[property: CommandSwitch("--link-name")] string LinkName,
[property: CommandSwitch("--perimeter-name")] string PerimeterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--auto-remote-nsp-id")]
    public string? AutoRemoteNspId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--local-inbound-profile")]
    public string? LocalInboundProfile { get; set; }

    [CommandSwitch("--remote-inbound-profile")]
    public string? RemoteInboundProfile { get; set; }
}