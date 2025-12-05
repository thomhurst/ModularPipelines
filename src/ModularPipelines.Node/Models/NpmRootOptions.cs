using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("root")]
public record NpmRootOptions : NpmOptions
{
    [CliFlag("--global")]
    public virtual bool? Global { get; set; }
}