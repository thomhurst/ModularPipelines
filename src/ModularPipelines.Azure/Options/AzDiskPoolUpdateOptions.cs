using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("disk-pool", "update")]
public record AzDiskPoolUpdateOptions : AzOptions
{
    [CliOption("--disk-pool-name")]
    public string? DiskPoolName { get; set; }

    [CliOption("--disks")]
    public string? Disks { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--managed-by")]
    public string? ManagedBy { get; set; }

    [CliOption("--managed-by-extended")]
    public string? ManagedByExtended { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}