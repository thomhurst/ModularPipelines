using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("version")]
public record YarnVersionOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Strategy
) : YarnOptions
{
    [CliFlag("--deferred")]
    public virtual bool? Deferred { get; set; }

    [CliFlag("--immediate")]
    public virtual bool? Immediate { get; set; }
}