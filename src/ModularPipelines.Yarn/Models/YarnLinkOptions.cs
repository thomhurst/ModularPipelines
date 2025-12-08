using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("link")]
public record YarnLinkOptions : YarnOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--private")]
    public virtual bool? Private { get; set; }

    [CliFlag("--relative")]
    public virtual bool? Relative { get; set; }
}