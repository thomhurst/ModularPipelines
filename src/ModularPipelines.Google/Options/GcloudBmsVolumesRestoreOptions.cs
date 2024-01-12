using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "volumes", "restore")]
public record GcloudBmsVolumesRestoreOptions(
[property: PositionalArgument] string Volume,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--snapshot")] string Snapshot
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}