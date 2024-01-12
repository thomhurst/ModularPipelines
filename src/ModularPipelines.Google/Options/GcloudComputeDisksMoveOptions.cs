using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "disks", "move")]
public record GcloudComputeDisksMoveOptions(
[property: PositionalArgument] string DiskName,
[property: CommandSwitch("--destination-zone")] string DestinationZone
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}