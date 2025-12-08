using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("ci")]
public record NpmCiOptions : NpmOptions
{
    [CliOption("--install-strategy")]
    public virtual string? InstallStrategy { get; set; }

    [CliOption("--omit")]
    public virtual string? Omit { get; set; }

    [CliOption("--include")]
    public virtual string? Include { get; set; }

    [CliFlag("--strict-peer-deps")]
    public virtual bool? StrictPeerDeps { get; set; }

    [CliFlag("--foreground-scripts")]
    public virtual bool? ForegroundScripts { get; set; }

    [CliFlag("--ignore-scripts")]
    public virtual bool? IgnoreScripts { get; set; }

    [CliFlag("--audit")]
    public virtual bool? Audit { get; set; }

    [CliFlag("--bin-links")]
    public virtual bool? BinLinks { get; set; }

    [CliFlag("--fund")]
    public virtual bool? Fund { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliOption("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [CliFlag("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [CliFlag("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }

    [CliFlag("--install-links")]
    public virtual bool? InstallLinks { get; set; }
}