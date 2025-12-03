using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("pkg", "set")]
public record NpmPkgSetOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Value
) : NpmOptions
{
    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliOption("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [CliFlag("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Key { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Array { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Index { get; set; }
}