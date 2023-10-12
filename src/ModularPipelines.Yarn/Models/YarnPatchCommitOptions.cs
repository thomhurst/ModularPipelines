using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("patch-commit")]
public record YarnPatchCommitOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string PatchFolder
) : YarnOptions
{
    [BooleanCommandSwitch("--save")]
    public bool? Save { get; set; }
}