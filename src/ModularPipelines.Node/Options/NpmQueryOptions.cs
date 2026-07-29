using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("query")]
public record NpmQueryOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Selector
) : NpmOptions
{
    [CliFlag("--global")]
    public virtual bool? Global { get; set; }

    [CliOption("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [CliFlag("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [CliFlag("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }

    [CliFlag("--package-lock-only")]
    public virtual bool? PackageLockOnly { get; set; }
}