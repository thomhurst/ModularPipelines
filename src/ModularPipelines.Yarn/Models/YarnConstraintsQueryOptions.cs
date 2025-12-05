using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("constraints", "query")]
public record YarnConstraintsQueryOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Query
) : YarnOptions
{
    [CliFlag("--json")]
    public virtual bool? Json { get; set; }
}