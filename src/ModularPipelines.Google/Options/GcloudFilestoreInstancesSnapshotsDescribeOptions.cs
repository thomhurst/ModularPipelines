using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("filestore", "instances", "snapshots", "describe")]
public record GcloudFilestoreInstancesSnapshotsDescribeOptions(
[property: PositionalArgument] string Snapshot,
[property: CommandSwitch("--instance")] string Instance,
[property: CommandSwitch("--instance-location")] string InstanceLocation,
[property: CommandSwitch("--instance-region")] string InstanceRegion
) : GcloudOptions;