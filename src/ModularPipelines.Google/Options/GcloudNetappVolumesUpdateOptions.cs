using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp", "volumes", "update")]
public record GcloudNetappVolumesUpdateOptions(
[property: PositionalArgument] string Volume,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--capacity")]
    public string? Capacity { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--enable-kerberos")]
    public string? EnableKerberos { get; set; }

    [CommandSwitch("--export-policy")]
    public string[]? ExportPolicy { get; set; }

    [CommandSwitch("--protocols")]
    public string[]? Protocols { get; set; }

    [CommandSwitch("--restricted-actions")]
    public string[]? RestrictedActions { get; set; }

    [CommandSwitch("--security-style")]
    public string? SecurityStyle { get; set; }

    [CommandSwitch("--share-name")]
    public string? ShareName { get; set; }

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

    [CommandSwitch("--storage-pool")]
    public string? StoragePool { get; set; }

    [CommandSwitch("--unix-permissions")]
    public string? UnixPermissions { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}