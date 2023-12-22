using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedvmware", "vcenter", "inventory-item", "show")]
public record AzConnectedvmwareVcenterInventoryItemShowOptions(
[property: CommandSwitch("--inventory-item")] string InventoryItem,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vcenter")] string Vcenter
) : AzOptions;