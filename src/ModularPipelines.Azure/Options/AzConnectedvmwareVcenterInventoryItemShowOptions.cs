using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedvmware", "vcenter", "inventory-item", "show")]
public record AzConnectedvmwareVcenterInventoryItemShowOptions(
[property: CommandSwitch("--inventory-item")] string InventoryItem,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vcenter")] string Vcenter
) : AzOptions
{
}

