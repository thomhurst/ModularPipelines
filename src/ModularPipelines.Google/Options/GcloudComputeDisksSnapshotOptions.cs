using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "disks", "snapshot")]
public record GcloudComputeDisksSnapshotOptions(
[property: CliArgument] string DiskName
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--chain-name")]
    public string? ChainName { get; set; }

    [CliOption("--csek-key-file")]
    public string? CsekKeyFile { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--guest-flush")]
    public bool? GuestFlush { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--snapshot-names")]
    public string[]? SnapshotNames { get; set; }

    [CliOption("--storage-location")]
    public string? StorageLocation { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}