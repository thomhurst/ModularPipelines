using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("explore", "--")]
public record NpmExploreOptions : NpmOptions
{
    [CliOption("--shell")]
    public virtual string? Shell { get; set; }
}
