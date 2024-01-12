using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("filestore", "instances", "snapshots", "list")]
public record GcloudFilestoreInstancesSnapshotsListOptions(
[property: CommandSwitch("--instance")] string Instance,
[property: CommandSwitch("--instance-location")] string InstanceLocation,
[property: CommandSwitch("--instance-region")] string InstanceRegion
) : GcloudOptions;