using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("filestore", "instances", "snapshots", "update")]
public record GcloudFilestoreInstancesSnapshotsUpdateOptions(
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

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}