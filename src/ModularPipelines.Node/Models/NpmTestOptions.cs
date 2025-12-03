using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("test")]
public record NpmTestOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Args
) : NpmOptions
{
    [CliFlag("--ignore-scripts")]
    public virtual bool? IgnoreScripts { get; set; }

    [CliOption("--script-shell")]
    public virtual string? ScriptShell { get; set; }
}