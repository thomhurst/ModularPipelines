using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("filestore", "instances", "snapshots", "list")]
public record GcloudFilestoreInstancesSnapshotsListOptions(
[property: CliOption("--instance")] string Instance,
[property: CliOption("--instance-location")] string InstanceLocation,
[property: CliOption("--instance-region")] string InstanceRegion
) : GcloudOptions;