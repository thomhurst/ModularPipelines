using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scvmm", "vmmserver", "inventory-item", "show")]
public record AzScvmmVmmserverInventoryItemShowOptions(
[property: CliOption("--inventory-item")] string InventoryItem,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vmmserver")] string Vmmserver
) : AzOptions;