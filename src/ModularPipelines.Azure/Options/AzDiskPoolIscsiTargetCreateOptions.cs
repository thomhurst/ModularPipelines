using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("disk-pool", "iscsi-target", "create")]
public record AzDiskPoolIscsiTargetCreateOptions(
[property: CliOption("--acl-mode")] string AclMode,
[property: CliOption("--disk-pool-name")] string DiskPoolName,
[property: CliOption("--iscsi-target-name")] string IscsiTargetName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--luns")]
    public string? Luns { get; set; }

    [CliOption("--managed-by")]
    public string? ManagedBy { get; set; }

    [CliOption("--managed-by-extended")]
    public string? ManagedByExtended { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--static-acls")]
    public string? StaticAcls { get; set; }

    [CliOption("--target-iqn")]
    public string? TargetIqn { get; set; }
}