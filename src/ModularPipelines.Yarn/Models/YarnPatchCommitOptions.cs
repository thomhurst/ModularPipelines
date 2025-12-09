using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("patch-commit")]
public record YarnPatchCommitOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string PatchFolder
) : YarnOptions
{
    [CliFlag("--save")]
    public virtual bool? Save { get; set; }
}