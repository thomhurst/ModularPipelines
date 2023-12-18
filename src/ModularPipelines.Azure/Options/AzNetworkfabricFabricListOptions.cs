using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "fabric", "list")]
public record AzNetworkfabricFabricListOptions : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}