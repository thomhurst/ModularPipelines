using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "disks", "snapshot")]
public record GcloudComputeDisksSnapshotOptions(
[property: PositionalArgument] string DiskName
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

    [CommandSwitch("--snapshot-names")]
    public string[]? SnapshotNames { get; set; }

    [CommandSwitch("--storage-location")]
    public string? StorageLocation { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}