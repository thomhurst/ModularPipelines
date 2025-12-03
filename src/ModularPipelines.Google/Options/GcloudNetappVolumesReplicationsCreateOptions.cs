using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "volumes", "replications", "create")]
public record GcloudNetappVolumesReplicationsCreateOptions(
[property: CliArgument] string Replication,
[property: CliArgument] string Location,
[property: CliOption("--destination-volume-parameters")] string[] DestinationVolumeParameters,
[property: CliOption("--replication-schedule")] string ReplicationSchedule
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--volume")]
    public string? Volume { get; set; }
}