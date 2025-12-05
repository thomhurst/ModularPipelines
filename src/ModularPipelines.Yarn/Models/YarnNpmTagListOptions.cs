using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("npm", "tag", "list")]
public record YarnNpmTagListOptions : YarnOptions
{
    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public virtual string? Package { get; set; }
}