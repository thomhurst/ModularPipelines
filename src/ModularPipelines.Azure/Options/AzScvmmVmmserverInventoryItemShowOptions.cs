using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scvmm", "vmmserver", "inventory-item", "show")]
public record AzScvmmVmmserverInventoryItemShowOptions(
[property: CommandSwitch("--inventory-item")] string InventoryItem,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vmmserver")] string Vmmserver
) : AzOptions;