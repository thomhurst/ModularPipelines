using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "datastore", "netapp-volume", "create")]
public record AzVmwareDatastoreNetappVolumeCreateOptions(
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--datastore-name")] string DatastoreName,
[property: CliOption("--net-app-volumn")] string NetAppVolumn,
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}