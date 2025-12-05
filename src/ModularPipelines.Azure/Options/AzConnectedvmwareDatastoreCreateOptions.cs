using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("connectedvmware", "datastore", "create")]
public record AzConnectedvmwareDatastoreCreateOptions(
[property: CliOption("--custom-location")] string CustomLocation,
[property: CliOption("--inventory-item")] string InventoryItem,
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vcenter")] string Vcenter
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}