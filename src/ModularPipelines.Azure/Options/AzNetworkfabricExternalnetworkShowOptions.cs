using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "externalnetwork", "show")]
public record AzNetworkfabricExternalnetworkShowOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--l3-isolation-domain-name")]
    public string? L3IsolationDomainName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-name")]
    public string? ResourceName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}