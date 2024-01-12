using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "tpu-vm", "update")]
public record GcloudComputeTpusTpuVmUpdateOptions(
[property: PositionalArgument] string Tpu,
[property: PositionalArgument] string Zone
) : GcloudOptions
{
    [CommandSwitch("--add-tags")]
    public string[]? AddTags { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--internal-ips")]
    public bool? InternalIps { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CommandSwitch("--attach-disk")]
    public string[]? AttachDisk { get; set; }

    [BooleanCommandSwitch("source")]
    public bool? Source { get; set; }

    [BooleanCommandSwitch("mode")]
    public bool? Mode { get; set; }

    [CommandSwitch("--detach-disk")]
    public string? DetachDisk { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [BooleanCommandSwitch("--clear-tags")]
    public bool? ClearTags { get; set; }

    [CommandSwitch("--remove-tags")]
    public string[]? RemoveTags { get; set; }

    [CommandSwitch("--metadata-from-file")]
    public IEnumerable<KeyValue>? MetadataFromFile { get; set; }

    [CommandSwitch("--update-metadata")]
    public IEnumerable<KeyValue>? UpdateMetadata { get; set; }

    [BooleanCommandSwitch("--clear-metadata")]
    public bool? ClearMetadata { get; set; }

    [CommandSwitch("--remove-metadata")]
    public string[]? RemoveMetadata { get; set; }
}