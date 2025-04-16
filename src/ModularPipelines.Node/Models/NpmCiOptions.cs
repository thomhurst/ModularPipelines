using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ci")]
public record NpmCiOptions : NpmOptions
{
    [CommandSwitch("--install-strategy")]
    public virtual string? InstallStrategy { get; set; }

    [CommandSwitch("--omit")]
    public virtual string? Omit { get; set; }

    [CommandSwitch("--include")]
    public virtual string? Include { get; set; }

    [BooleanCommandSwitch("--strict-peer-deps")]
    public virtual bool? StrictPeerDeps { get; set; }

    [BooleanCommandSwitch("--foreground-scripts")]
    public virtual bool? ForegroundScripts { get; set; }

    [BooleanCommandSwitch("--ignore-scripts")]
    public virtual bool? IgnoreScripts { get; set; }

    [BooleanCommandSwitch("--audit")]
    public virtual bool? Audit { get; set; }

    [BooleanCommandSwitch("--bin-links")]
    public virtual bool? BinLinks { get; set; }

    [BooleanCommandSwitch("--fund")]
    public virtual bool? Fund { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CommandSwitch("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }

    [BooleanCommandSwitch("--install-links")]
    public virtual bool? InstallLinks { get; set; }
}