using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("cache", "ls")]
public record NpmCacheLsOptions : NpmOptions
{
    [CliOption("--cache")]
    public virtual string? Cache { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Name { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Version { get; set; }
}