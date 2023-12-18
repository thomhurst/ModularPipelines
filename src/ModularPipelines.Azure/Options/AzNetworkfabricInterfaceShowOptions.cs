using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "interface", "show")]
public record AzNetworkfabricInterfaceShowOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-name")]
    public string? ResourceName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}