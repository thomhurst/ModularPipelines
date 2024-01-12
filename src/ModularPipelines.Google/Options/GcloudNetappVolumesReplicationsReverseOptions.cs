using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp", "volumes", "replications", "reverse")]
public record GcloudNetappVolumesReplicationsReverseOptions(
[property: PositionalArgument] string Replication,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--volume")]
    public string? Volume { get; set; }
}