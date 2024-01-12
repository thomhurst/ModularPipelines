using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "snapshots", "create")]
public record GcloudComputeSnapshotsCreateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--chain-name")]
    public string? ChainName { get; set; }

    [CommandSwitch("--csek-key-file")]
    public string? CsekKeyFile { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--guest-flush")]
    public bool? GuestFlush { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--snapshot-type")]
    public string? SnapshotType { get; set; }

    [CommandSwitch("--source-disk")]
    public string? SourceDisk { get; set; }

    [CommandSwitch("--source-disk-for-recovery-checkpoint")]
    public string? SourceDiskForRecoveryCheckpoint { get; set; }

    [CommandSwitch("--source-disk-for-recovery-checkpoint-region")]
    public string? SourceDiskForRecoveryCheckpointRegion { get; set; }

    [CommandSwitch("--source-disk-key-file")]
    public string? SourceDiskKeyFile { get; set; }

    [CommandSwitch("--storage-location")]
    public string? StorageLocation { get; set; }

    [CommandSwitch("--source-disk-region")]
    public string? SourceDiskRegion { get; set; }

    [CommandSwitch("--source-disk-zone")]
    public string? SourceDiskZone { get; set; }
}