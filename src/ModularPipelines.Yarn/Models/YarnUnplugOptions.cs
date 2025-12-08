using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("unplug")]
public record YarnUnplugOptions : YarnOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }
}