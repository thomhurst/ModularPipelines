using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("run")]
public record YarnRunOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string ScriptName
) : YarnOptions
{
    [CliFlag("--inspect")]
    public virtual bool? Inspect { get; set; }

    [CliFlag("--inspect-brk")]
    public virtual bool? InspectBrk { get; set; }

    [CliFlag("--top-level")]
    public virtual bool? TopLevel { get; set; }

    [CliFlag("--binaries-only")]
    public virtual bool? BinariesOnly { get; set; }

    [CliOption("--require")]
    public virtual string? Require { get; set; }
}