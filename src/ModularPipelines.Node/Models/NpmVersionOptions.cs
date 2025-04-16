using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("version")]
public record NpmVersionOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Value
) : NpmOptions
{
    [BooleanCommandSwitch("--allow-same-version")]
    public virtual bool? AllowSameVersion { get; set; }

    [BooleanCommandSwitch("--commit-hooks")]
    public virtual bool? CommitHooks { get; set; }

    [BooleanCommandSwitch("--git-tag-version")]
    public virtual bool? GitTagVersion { get; set; }

    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }

    [CommandSwitch("--preid")]
    public virtual string? Preid { get; set; }

    [BooleanCommandSwitch("--sign-git-tag")]
    public virtual bool? SignGitTag { get; set; }

    [CommandSwitch("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--workspaces-update")]
    public virtual bool? WorkspacesUpdate { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }
}