using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware", "datastore", "disk-pool-volume", "create")]
public record AzVmwareDatastoreDiskPoolVolumeCreateOptions(
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--datastore-name")] string DatastoreName,
[property: CliOption("--lun-name")] string LunName,
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--target-id")] string TargetId
) : AzOptions
{
    [CliOption("--mount-option")]
    public string? MountOption { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}