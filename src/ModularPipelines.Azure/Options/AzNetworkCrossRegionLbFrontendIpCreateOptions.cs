using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "cross-region-lb", "frontend-ip", "create")]
public record AzNetworkCrossRegionLbFrontendIpCreateOptions(
[property: CommandSwitch("--lb-name")] string LbName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--public-ip-address")]
    public string? PublicIpAddress { get; set; }

    [CommandSwitch("--public-ip-prefix")]
    public string? PublicIpPrefix { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}

