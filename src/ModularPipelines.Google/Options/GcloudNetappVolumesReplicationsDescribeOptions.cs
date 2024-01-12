using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp", "volumes", "replications", "describe")]
public record GcloudNetappVolumesReplicationsDescribeOptions(
[property: PositionalArgument] string Replication,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--volume")]
    public string? Volume { get; set; }
}