using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("root")]
public record NpmRootOptions : NpmOptions
{
    [CliFlag("--global")]
    public virtual bool? Global { get; set; }
}