using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp", "volumes", "revert")]
public record GcloudNetappVolumesRevertOptions(
[property: PositionalArgument] string Volume,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--snapshot")] string Snapshot
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}