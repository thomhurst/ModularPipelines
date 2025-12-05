using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("help")]
public record NpmHelpOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Term
) : NpmOptions
{
    [CliOption("--viewer")]
    public virtual string? Viewer { get; set; }
}