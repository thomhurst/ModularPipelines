using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("filestore", "instances", "snapshots", "create")]
public record GcloudFilestoreInstancesSnapshotsCreateOptions(
[property: CliArgument] string Snapshot,
[property: CliOption("--instance")] string Instance,
[property: CliOption("--instance-location")] string InstanceLocation,
[property: CliOption("--instance-region")] string InstanceRegion
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}