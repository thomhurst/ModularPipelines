using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("disk-pool", "create")]
public record AzDiskPoolCreateOptions(
[property: CommandSwitch("--disk-pool-name")] string DiskPoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sku")] string Sku,
[property: CommandSwitch("--subnet-id")] string SubnetId
) : AzOptions
{
    [CommandSwitch("--additional-capabilities")]
    public string? AdditionalCapabilities { get; set; }

    [CommandSwitch("--availability-zones")]
    public string? AvailabilityZones { get; set; }

    [CommandSwitch("--disks")]
    public string? Disks { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--managed-by")]
    public string? ManagedBy { get; set; }

    [CommandSwitch("--managed-by-extended")]
    public string? ManagedByExtended { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

