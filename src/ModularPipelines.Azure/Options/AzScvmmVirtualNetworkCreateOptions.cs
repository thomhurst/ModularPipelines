using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scvmm", "virtual-network", "create")]
public record AzScvmmVirtualNetworkCreateOptions(
[property: CliOption("--custom-location")] string CustomLocation,
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--inventory-item")]
    public string? InventoryItem { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--uuid")]
    public string? Uuid { get; set; }

    [CliOption("--vmmserver")]
    public string? Vmmserver { get; set; }
}