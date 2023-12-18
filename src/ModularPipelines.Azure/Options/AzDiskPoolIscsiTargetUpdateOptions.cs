using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("disk-pool", "iscsi-target", "update")]
public record AzDiskPoolIscsiTargetUpdateOptions : AzOptions
{
    [CommandSwitch("--disk-pool-name")]
    public string? DiskPoolName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--iscsi-target-name")]
    public string? IscsiTargetName { get; set; }

    [CommandSwitch("--luns")]
    public string? Luns { get; set; }

    [CommandSwitch("--managed-by")]
    public string? ManagedBy { get; set; }

    [CommandSwitch("--managed-by-extended")]
    public string? ManagedByExtended { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--static-acls")]
    public string? StaticAcls { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}