using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("link")]
public record NpmLinkOptions : NpmOptions
{
    [CliFlag("--save")]
    public virtual bool? Save { get; set; }

    [CliFlag("--save-exact")]
    public virtual bool? SaveExact { get; set; }

    [CliFlag("--global")]
    public virtual bool? Global { get; set; }

    [CliOption("--install-strategy")]
    public virtual string? InstallStrategy { get; set; }

    [CliFlag("--strict-peer-deps")]
    public virtual bool? StrictPeerDeps { get; set; }

    [CliFlag("--package-lock")]
    public virtual bool? PackageLock { get; set; }

    [CliOption("--omit")]
    public virtual string? Omit { get; set; }

    [CliOption("--include")]
    public virtual string? Include { get; set; }

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