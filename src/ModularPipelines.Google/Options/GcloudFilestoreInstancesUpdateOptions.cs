using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("filestore", "instances", "update")]
public record GcloudFilestoreInstancesUpdateOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Zone
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliOption("--file-share")]
    public string[]? FileShare { get; set; }

    [CliFlag("capacity")]
    public bool? Capacity { get; set; }

    [CliFlag("name")]
    public bool? Name { get; set; }

    [CliFlag("nfs-export-options")]
    public bool? NfsExportOptions { get; set; }

    [CliFlag("ip-ranges")]
    public bool? IpRanges { get; set; }

    [CliFlag("access-mode")]
    public bool? AccessMode { get; set; }

    [CliFlag("squash-mode")]
    public bool? SquashMode { get; set; }

    [CliFlag("anon_uid")]
    public bool? AnonUid { get; set; }

    [CliFlag("anon_gid")]
    public bool? AnonGid { get; set; }

    [CliFlag("--clear-nfs-export-options")]
    public bool? ClearNfsExportOptions { get; set; }
}