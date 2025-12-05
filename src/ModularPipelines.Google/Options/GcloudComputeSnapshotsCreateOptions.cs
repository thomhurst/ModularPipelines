using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "snapshots", "create")]
public record GcloudComputeSnapshotsCreateOptions(
[property: CliArgument] string Name
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

    [CliOption("--snapshot-type")]
    public string? SnapshotType { get; set; }

    [CliOption("--source-disk")]
    public string? SourceDisk { get; set; }

    [CliOption("--source-disk-for-recovery-checkpoint")]
    public string? SourceDiskForRecoveryCheckpoint { get; set; }

    [CliOption("--source-disk-for-recovery-checkpoint-region")]
    public string? SourceDiskForRecoveryCheckpointRegion { get; set; }

    [CliOption("--source-disk-key-file")]
    public string? SourceDiskKeyFile { get; set; }

    [CliOption("--storage-location")]
    public string? StorageLocation { get; set; }

    [CliOption("--source-disk-region")]
    public string? SourceDiskRegion { get; set; }

    [CliOption("--source-disk-zone")]
    public string? SourceDiskZone { get; set; }
}