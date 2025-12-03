using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectedvmware", "vm", "create")]
public record AzConnectedvmwareVmCreateOptions(
[property: CliOption("--custom-location")] string CustomLocation,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--admin-password")]
    public string? AdminPassword { get; set; }

    [CliOption("--admin-username")]
    public string? AdminUsername { get; set; }

    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--datastore")]
    public string? Datastore { get; set; }

    [CliOption("--disk")]
    public string? Disk { get; set; }

    [CliOption("--host")]
    public string? Host { get; set; }

    [CliOption("--inventory-item")]
    public string? InventoryItem { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--memory-size")]
    public string? MemorySize { get; set; }

    [CliOption("--nic")]
    public string? Nic { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--num-CPUs")]
    public string? NumCPUs { get; set; }

    [CliOption("--num-cores-per-socket")]
    public string? NumCoresPerSocket { get; set; }

    [CliOption("--resource-pool")]
    public string? ResourcePool { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vcenter")]
    public string? Vcenter { get; set; }

    [CliOption("--vm-template")]
    public string? VmTemplate { get; set; }
}