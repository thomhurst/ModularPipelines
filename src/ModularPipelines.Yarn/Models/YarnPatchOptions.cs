using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("patch")]
public record YarnPatchOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Package
) : YarnOptions
{
    [CliFlag("--update")]
    public virtual bool? Update { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }
}