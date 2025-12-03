using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("version")]
public record NpmVersionOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Value
) : NpmOptions
{
    [CliFlag("--allow-same-version")]
    public virtual bool? AllowSameVersion { get; set; }

    [CliFlag("--commit-hooks")]
    public virtual bool? CommitHooks { get; set; }

    [CliFlag("--git-tag-version")]
    public virtual bool? GitTagVersion { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliOption("--preid")]
    public virtual string? Preid { get; set; }

    [CliFlag("--sign-git-tag")]
    public virtual bool? SignGitTag { get; set; }

    [CliOption("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [CliFlag("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [CliFlag("--workspaces-update")]
    public virtual bool? WorkspacesUpdate { get; set; }

    [CliFlag("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }
}