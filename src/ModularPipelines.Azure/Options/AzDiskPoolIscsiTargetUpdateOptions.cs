using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("disk-pool", "iscsi-target", "update")]
public record AzDiskPoolIscsiTargetUpdateOptions : AzOptions
{
    [CliOption("--disk-pool-name")]
    public string? DiskPoolName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--iscsi-target-name")]
    public string? IscsiTargetName { get; set; }

    [CliOption("--luns")]
    public string? Luns { get; set; }

    [CliOption("--managed-by")]
    public string? ManagedBy { get; set; }

    [CliOption("--managed-by-extended")]
    public string? ManagedByExtended { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--static-acls")]
    public string? StaticAcls { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}