using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scvmm", "vm", "create")]
public record AzScvmmVmCreateOptions(
[property: CliOption("--custom-location")] string CustomLocation,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--admin-password")]
    public string? AdminPassword { get; set; }

    [CliOption("--availability-sets")]
    public string? AvailabilitySets { get; set; }

    [CliOption("--cloud")]
    public string? Cloud { get; set; }

    [CliOption("--cpu-count")]
    public int? CpuCount { get; set; }

    [CliOption("--disk")]
    public string? Disk { get; set; }

    [CliFlag("--dynamic-memory-enabled")]
    public bool? DynamicMemoryEnabled { get; set; }

    [CliOption("--dynamic-memory-max")]
    public string? DynamicMemoryMax { get; set; }

    [CliOption("--dynamic-memory-min")]
    public string? DynamicMemoryMin { get; set; }

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

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vm-template")]
    public string? VmTemplate { get; set; }

    [CliOption("--vmmserver")]
    public string? Vmmserver { get; set; }
}