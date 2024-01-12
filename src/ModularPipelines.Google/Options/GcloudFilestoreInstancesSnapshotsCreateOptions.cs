using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("filestore", "instances", "snapshots", "create")]
public record GcloudFilestoreInstancesSnapshotsCreateOptions(
[property: PositionalArgument] string Snapshot,
[property: CommandSwitch("--instance")] string Instance,
[property: CommandSwitch("--instance-location")] string InstanceLocation,
[property: CommandSwitch("--instance-region")] string InstanceRegion
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}