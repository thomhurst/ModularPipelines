using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("prefix")]
public record NpmPrefixOptions : NpmOptions
{
    [CliFlag("--global")]
    public virtual bool? Global { get; set; }
}