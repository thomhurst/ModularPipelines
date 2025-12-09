using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("config", "unset")]
public record YarnConfigUnsetOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Name
) : YarnOptions
{
    [CliFlag("--home")]
    public virtual bool? Home { get; set; }
}