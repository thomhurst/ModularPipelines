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
    public bool? AllowSameVersion { get; set; }

    [BooleanCommandSwitch("--commit-hooks")]
    public bool? CommitHooks { get; set; }

    [BooleanCommandSwitch("--git-tag-version")]
    public bool? GitTagVersion { get; set; }

    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [CommandSwitch("--preid")]
    public string? Preid { get; set; }

    [BooleanCommandSwitch("--sign-git-tag")]
    public bool? SignGitTag { get; set; }

    [CommandSwitch("--workspace")]
    public string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--workspaces-update")]
    public bool? WorkspacesUpdate { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public bool? IncludeWorkspaceRoot { get; set; }
}