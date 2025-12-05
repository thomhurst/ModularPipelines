using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("audit")]
public record NpmAuditOptions : NpmOptions
{
    [CliOption("--audit-level")]
    public virtual string? AuditLevel { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliFlag("--package-lock-only")]
    public virtual bool? PackageLockOnly { get; set; }

    [CliFlag("--package-lock")]
    public virtual bool? PackageLock { get; set; }

    [CliOption("--omit")]
    public virtual string? Omit { get; set; }

    [CliOption("--include")]
    public virtual string? Include { get; set; }

    [CliFlag("--foreground-scripts")]
    public virtual bool? ForegroundScripts { get; set; }

    [CliFlag("--ignore-scripts")]
    public virtual bool? IgnoreScripts { get; set; }

    [CliOption("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [CliFlag("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [CliFlag("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }

    [CliFlag("--install-links")]
    public virtual bool? InstallLinks { get; set; }
}