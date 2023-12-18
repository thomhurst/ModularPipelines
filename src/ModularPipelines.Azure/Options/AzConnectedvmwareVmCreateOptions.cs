using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedvmware", "vm", "create")]
public record AzConnectedvmwareVmCreateOptions(
[property: CommandSwitch("--custom-location")] string CustomLocation,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--admin-password")]
    public string? AdminPassword { get; set; }

    [CommandSwitch("--admin-username")]
    public string? AdminUsername { get; set; }

    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--datastore")]
    public string? Datastore { get; set; }

    [CommandSwitch("--disk")]
    public string? Disk { get; set; }

    [CommandSwitch("--host")]
    public string? Host { get; set; }

    [CommandSwitch("--inventory-item")]
    public string? InventoryItem { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--memory-size")]
    public string? MemorySize { get; set; }

    [CommandSwitch("--nic")]
    public string? Nic { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--num-CPUs")]
    public string? NumCPUs { get; set; }

    [CommandSwitch("--num-cores-per-socket")]
    public string? NumCoresPerSocket { get; set; }

    [CommandSwitch("--resource-pool")]
    public string? ResourcePool { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vcenter")]
    public string? Vcenter { get; set; }

    [CommandSwitch("--vm-template")]
    public string? VmTemplate { get; set; }
}