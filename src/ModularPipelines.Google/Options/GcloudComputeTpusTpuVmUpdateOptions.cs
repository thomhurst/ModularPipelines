using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "tpu-vm", "update")]
public record GcloudComputeTpusTpuVmUpdateOptions(
[property: CliArgument] string Tpu,
[property: CliArgument] string Zone
) : GcloudOptions
{
    [CliOption("--add-tags")]
    public string[]? AddTags { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--internal-ips")]
    public bool? InternalIps { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliOption("--attach-disk")]
    public string[]? AttachDisk { get; set; }

    [CliFlag("source")]
    public bool? Source { get; set; }

    [CliFlag("mode")]
    public bool? Mode { get; set; }

    [CliOption("--detach-disk")]
    public string? DetachDisk { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliFlag("--clear-tags")]
    public bool? ClearTags { get; set; }

    [CliOption("--remove-tags")]
    public string[]? RemoveTags { get; set; }

    [CliOption("--metadata-from-file")]
    public IEnumerable<KeyValue>? MetadataFromFile { get; set; }

    [CliOption("--update-metadata")]
    public IEnumerable<KeyValue>? UpdateMetadata { get; set; }

    [CliFlag("--clear-metadata")]
    public bool? ClearMetadata { get; set; }

    [CliOption("--remove-metadata")]
    public string[]? RemoveMetadata { get; set; }
}