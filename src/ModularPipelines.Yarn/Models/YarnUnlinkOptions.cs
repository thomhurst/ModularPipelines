using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("unlink")]
public record YarnUnlinkOptions : YarnOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }
}