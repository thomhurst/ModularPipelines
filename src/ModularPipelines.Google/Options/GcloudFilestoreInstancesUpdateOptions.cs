using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("filestore", "instances", "update")]
public record GcloudFilestoreInstancesUpdateOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Zone
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CommandSwitch("--file-share")]
    public string[]? FileShare { get; set; }

    [BooleanCommandSwitch("capacity")]
    public bool? Capacity { get; set; }

    [BooleanCommandSwitch("name")]
    public bool? Name { get; set; }

    [BooleanCommandSwitch("nfs-export-options")]
    public bool? NfsExportOptions { get; set; }

    [BooleanCommandSwitch("ip-ranges")]
    public bool? IpRanges { get; set; }

    [BooleanCommandSwitch("access-mode")]
    public bool? AccessMode { get; set; }

    [BooleanCommandSwitch("squash-mode")]
    public bool? SquashMode { get; set; }

    [BooleanCommandSwitch("anon_uid")]
    public bool? AnonUid { get; set; }

    [BooleanCommandSwitch("anon_gid")]
    public bool? AnonGid { get; set; }

    [BooleanCommandSwitch("--clear-nfs-export-options")]
    public bool? ClearNfsExportOptions { get; set; }
}