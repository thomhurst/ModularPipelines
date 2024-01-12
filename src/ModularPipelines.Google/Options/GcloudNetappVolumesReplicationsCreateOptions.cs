using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp", "volumes", "replications", "create")]
public record GcloudNetappVolumesReplicationsCreateOptions(
[property: PositionalArgument] string Replication,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--destination-volume-parameters")] string[] DestinationVolumeParameters,
[property: CommandSwitch("--replication-schedule")] string ReplicationSchedule
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--volume")]
    public string? Volume { get; set; }
}