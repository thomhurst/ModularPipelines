using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("disk-pool", "iscsi-target", "create")]
public record AzDiskPoolIscsiTargetCreateOptions(
[property: CommandSwitch("--acl-mode")] string AclMode,
[property: CommandSwitch("--disk-pool-name")] string DiskPoolName,
[property: CommandSwitch("--iscsi-target-name")] string IscsiTargetName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--luns")]
    public string? Luns { get; set; }

    [CommandSwitch("--managed-by")]
    public string? ManagedBy { get; set; }

    [CommandSwitch("--managed-by-extended")]
    public string? ManagedByExtended { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--static-acls")]
    public string? StaticAcls { get; set; }

    [CommandSwitch("--target-iqn")]
    public string? TargetIqn { get; set; }
}