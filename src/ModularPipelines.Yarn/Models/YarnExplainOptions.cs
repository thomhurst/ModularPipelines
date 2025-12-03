using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("explain")]
public record YarnExplainOptions : YarnOptions
{
    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Code { get; set; }
}