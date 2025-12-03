using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("edit")]
public record NpmEditOptions : NpmOptions
{
    [CliOption("--editor")]
    public virtual string? Editor { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Pkg { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Subpkg { get; set; }
}