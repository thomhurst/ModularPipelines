using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("filestore", "instances", "snapshots", "delete")]
public record GcloudFilestoreInstancesSnapshotsDeleteOptions(
[property: PositionalArgument] string Snapshot,
[property: CommandSwitch("--instance")] string Instance,
[property: CommandSwitch("--instance-location")] string InstanceLocation,
[property: CommandSwitch("--instance-region")] string InstanceRegion
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}