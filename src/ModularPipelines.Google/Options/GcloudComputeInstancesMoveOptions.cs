using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "move")]
public record GcloudComputeInstancesMoveOptions(
[property: CliArgument] string InstanceName,
[property: CliOption("--destination-zone")] string DestinationZone
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}