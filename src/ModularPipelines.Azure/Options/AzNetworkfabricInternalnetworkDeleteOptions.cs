using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "internalnetwork", "delete")]
public record AzNetworkfabricInternalnetworkDeleteOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--l3-isolation-domain-name")]
    public string? L3IsolationDomainName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-name")]
    public string? ResourceName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}