using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("cache", "add")]
public record NpmCacheAddOptions : NpmOptions
{
    [CliOption("--cache")]
    public virtual string? Cache { get; set; }
}
