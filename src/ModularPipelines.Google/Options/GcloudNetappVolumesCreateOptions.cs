using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp", "volumes", "create")]
public record GcloudNetappVolumesCreateOptions(
[property: PositionalArgument] string Volume,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--capacity")] string Capacity,
[property: CommandSwitch("--protocols")] string[] Protocols,
[property: CommandSwitch("--share-name")] string ShareName,
[property: CommandSwitch("--storage-pool")] string StoragePool
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--enable-kerberos")]
    public string? EnableKerberos { get; set; }

    [CommandSwitch("--export-policy")]
    public string[]? ExportPolicy { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--restricted-actions")]
    public string[]? RestrictedActions { get; set; }

    [CommandSwitch("--security-style")]
    public string? SecurityStyle { get; set; }

    [CommandSwitch("--smb-settings")]
    public string[]? SmbSettings { get; set; }

    [CommandSwitch("--snap-reserve")]
    public string? SnapReserve { get; set; }

    [CommandSwitch("--snapshot-daily")]
    public string[]? SnapshotDaily { get; set; }

    [CommandSwitch("--snapshot-directory")]
    public string? SnapshotDirectory { get; set; }

    [CommandSwitch("--snapshot-hourly")]
    public string[]? SnapshotHourly { get; set; }

    [CommandSwitch("--snapshot-monthly")]
    public string[]? SnapshotMonthly { get; set; }

    [CommandSwitch("--snapshot-weekly")]
    public string[]? SnapshotWeekly { get; set; }

    [CommandSwitch("--source-snapshot")]
    public string? SourceSnapshot { get; set; }

    [CommandSwitch("--unix-permissions")]
    public string? UnixPermissions { get; set; }
}