using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("connectedvmware", "vcenter", "inventory-item", "list")]
public record AzConnectedvmwareVcenterInventoryItemListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vcenter")] string Vcenter
) : AzOptions;