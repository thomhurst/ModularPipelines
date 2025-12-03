using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("cache", "clean")]
public record NpmCacheCleanOptions : NpmOptions
{
    [CliOption("--cache")]
    public virtual string? Cache { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Key { get; set; }
}