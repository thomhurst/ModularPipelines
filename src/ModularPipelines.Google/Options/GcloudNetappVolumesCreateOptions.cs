using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "volumes", "create")]
public record GcloudNetappVolumesCreateOptions(
[property: CliArgument] string Volume,
[property: CliArgument] string Location,
[property: CliOption("--capacity")] string Capacity,
[property: CliOption("--protocols")] string[] Protocols,
[property: CliOption("--share-name")] string ShareName,
[property: CliOption("--storage-pool")] string StoragePool
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--enable-kerberos")]
    public string? EnableKerberos { get; set; }

    [CliOption("--export-policy")]
    public string[]? ExportPolicy { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--restricted-actions")]
    public string[]? RestrictedActions { get; set; }

    [CliOption("--security-style")]
    public string? SecurityStyle { get; set; }

    [CliOption("--smb-settings")]
    public string[]? SmbSettings { get; set; }

    [CliOption("--snap-reserve")]
    public string? SnapReserve { get; set; }

    [CliOption("--snapshot-daily")]
    public string[]? SnapshotDaily { get; set; }

    [CliOption("--snapshot-directory")]
    public string? SnapshotDirectory { get; set; }

    [CliOption("--snapshot-hourly")]
    public string[]? SnapshotHourly { get; set; }

    [CliOption("--snapshot-monthly")]
    public string[]? SnapshotMonthly { get; set; }

    [CliOption("--snapshot-weekly")]
    public string[]? SnapshotWeekly { get; set; }

    [CliOption("--source-snapshot")]
    public string? SourceSnapshot { get; set; }

    [CliOption("--unix-permissions")]
    public string? UnixPermissions { get; set; }
}