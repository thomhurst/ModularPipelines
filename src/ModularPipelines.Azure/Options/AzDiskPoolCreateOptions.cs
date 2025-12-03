using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("disk-pool", "create")]
public record AzDiskPoolCreateOptions(
[property: CliOption("--disk-pool-name")] string DiskPoolName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku,
[property: CliOption("--subnet-id")] string SubnetId
) : AzOptions
{
    [CliOption("--additional-capabilities")]
    public string? AdditionalCapabilities { get; set; }

    [CliOption("--availability-zones")]
    public string? AvailabilityZones { get; set; }

    [CliOption("--disks")]
    public string? Disks { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-by")]
    public string? ManagedBy { get; set; }

    [CliOption("--managed-by-extended")]
    public string? ManagedByExtended { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}