using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("filestore", "instances", "snapshots", "delete")]
public record GcloudFilestoreInstancesSnapshotsDeleteOptions(
[property: CliArgument] string Snapshot,
[property: CliOption("--instance")] string Instance,
[property: CliOption("--instance-location")] string InstanceLocation,
[property: CliOption("--instance-region")] string InstanceRegion
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}