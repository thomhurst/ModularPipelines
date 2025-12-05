using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("config", "set")]
public record YarnConfigSetOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Name,
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Value
) : YarnOptions
{
    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliFlag("--home")]
    public virtual bool? Home { get; set; }
}