using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-site", "delete")]
public record AzNetworkVpnSiteDeleteOptions(
[property: CommandSwitch("--request")] string Request,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vwan-name")] string VwanName
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

