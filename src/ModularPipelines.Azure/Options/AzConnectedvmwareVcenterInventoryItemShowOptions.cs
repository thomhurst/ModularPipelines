using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectedvmware", "vcenter", "inventory-item", "show")]
public record AzConnectedvmwareVcenterInventoryItemShowOptions(
[property: CliOption("--inventory-item")] string InventoryItem,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vcenter")] string Vcenter
) : AzOptions;