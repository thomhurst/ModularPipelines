using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("why")]
public record YarnWhyOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Package
) : YarnOptions
{
    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliFlag("--peers")]
    public virtual bool? Peers { get; set; }
}