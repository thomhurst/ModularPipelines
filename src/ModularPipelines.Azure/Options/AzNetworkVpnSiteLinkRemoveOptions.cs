using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-site", "link", "remove")]
public record AzNetworkVpnSiteLinkRemoveOptions(
[property: CommandSwitch("--index")] string Index
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--site-name")]
    public string? SiteName { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}