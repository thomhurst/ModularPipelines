using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("prune")]
public record NpmPruneOptions : NpmOptions
{
    [CommandSwitch("--omit")]
    public virtual string? Omit { get; set; }

    [CommandSwitch("--include")]
    public virtual string? Include { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }

    [BooleanCommandSwitch("--foreground-scripts")]
    public virtual bool? ForegroundScripts { get; set; }

    [BooleanCommandSwitch("--ignore-scripts")]
    public virtual bool? IgnoreScripts { get; set; }

    [CommandSwitch("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }

    [BooleanCommandSwitch("--install-links")]
    public virtual bool? InstallLinks { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Pkg { get; set; }
}