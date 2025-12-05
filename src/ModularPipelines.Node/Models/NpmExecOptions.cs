using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("exec", "--")]
public record NpmExecOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Value,
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Cmd
) : NpmOptions
{
    [CliOption("--package")]
    public virtual string[]? Package { get; set; }

    [CliOption("--call")]
    public virtual string? Call { get; set; }

    [CliOption("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [CliFlag("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [CliFlag("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public virtual string? Pkg { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public virtual string? Version { get; set; }
}