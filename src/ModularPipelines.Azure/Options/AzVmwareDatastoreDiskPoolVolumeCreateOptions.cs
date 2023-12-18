using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "datastore", "disk-pool-volume", "create")]
public record AzVmwareDatastoreDiskPoolVolumeCreateOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--datastore-name")] string DatastoreName,
[property: CommandSwitch("--lun-name")] string LunName,
[property: CommandSwitch("--private-cloud")] string PrivateCloud,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--target-id")] string TargetId
) : AzOptions
{
    [CommandSwitch("--mount-option")]
    public string? MountOption { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}