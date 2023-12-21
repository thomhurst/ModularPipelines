using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "attached-data-network", "show")]
public record AzMobileNetworkAttachedDataNetworkShowOptions : AzOptions
{
    [CommandSwitch("--adn-name")]
    public string? AdnName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--pccp-name")]
    public string? PccpName { get; set; }

    [CommandSwitch("--pcdp-name")]
    public string? PcdpName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}