using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("config", "get")]
public record YarnConfigGetOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Name
) : YarnOptions
{
    [CliFlag("--why")]
    public virtual bool? Why { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliFlag("--no-redacted")]
    public virtual bool? NoRedacted { get; set; }
}