using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "disks", "move")]
public record GcloudComputeDisksMoveOptions(
[property: CliArgument] string DiskName,
[property: CliOption("--destination-zone")] string DestinationZone
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}