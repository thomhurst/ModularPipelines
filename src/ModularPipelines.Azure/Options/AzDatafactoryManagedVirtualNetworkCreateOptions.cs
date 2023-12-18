using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "managed-virtual-network", "create")]
public record AzDatafactoryManagedVirtualNetworkCreateOptions(
[property: CommandSwitch("--factory-name")] string FactoryName,
[property: CommandSwitch("--managed-virtual-network-name")] string ManagedVirtualNetworkName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }
}