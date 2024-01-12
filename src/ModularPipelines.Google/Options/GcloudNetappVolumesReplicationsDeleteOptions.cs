using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp", "volumes", "replications", "delete")]
public record GcloudNetappVolumesReplicationsDeleteOptions(
[property: PositionalArgument] string Replication,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--volume")]
    public string? Volume { get; set; }
}