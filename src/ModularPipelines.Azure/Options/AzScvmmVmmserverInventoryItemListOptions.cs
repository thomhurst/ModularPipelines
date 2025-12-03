using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scvmm", "vmmserver", "inventory-item", "list")]
public record AzScvmmVmmserverInventoryItemListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vmmserver")] string Vmmserver
) : AzOptions;