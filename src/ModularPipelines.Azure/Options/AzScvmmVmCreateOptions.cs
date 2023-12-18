using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scvmm", "vm", "create")]
public record AzScvmmVmCreateOptions(
[property: CommandSwitch("--custom-location")] string CustomLocation,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--admin-password")]
    public string? AdminPassword { get; set; }

    [CommandSwitch("--availability-sets")]
    public string? AvailabilitySets { get; set; }

    [CommandSwitch("--cloud")]
    public string? Cloud { get; set; }

    [CommandSwitch("--cpu-count")]
    public int? CpuCount { get; set; }

    [CommandSwitch("--disk")]
    public string? Disk { get; set; }

    [BooleanCommandSwitch("--dynamic-memory-enabled")]
    public bool? DynamicMemoryEnabled { get; set; }

    [CommandSwitch("--dynamic-memory-max")]
    public string? DynamicMemoryMax { get; set; }

    [CommandSwitch("--dynamic-memory-min")]
    public string? DynamicMemoryMin { get; set; }

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

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vm-template")]
    public string? VmTemplate { get; set; }

    [CommandSwitch("--vmmserver")]
    public string? Vmmserver { get; set; }
}

