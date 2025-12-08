using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("unlink")]
public record YarnUnlinkOptions : YarnOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }
}