using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "data-network", "wait")]
public record AzMobileNetworkDataNetworkWaitOptions : AzOptions
{
    [BooleanCommandSwitch("--created")]
    public bool? Created { get; set; }

    [CommandSwitch("--custom")]
    public string? Custom { get; set; }

    [CommandSwitch("--data-network-name")]
    public string? DataNetworkName { get; set; }

    [BooleanCommandSwitch("--deleted")]
    public bool? Deleted { get; set; }

    [BooleanCommandSwitch("--exists")]
    public bool? Exists { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--interval")]
    public int? Interval { get; set; }

    [CommandSwitch("--mobile-network-name")]
    public string? MobileNetworkName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--updated")]
    public bool? Updated { get; set; }
}

