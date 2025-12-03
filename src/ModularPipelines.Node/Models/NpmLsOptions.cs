using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("ls")]
public record NpmLsOptions : NpmOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliFlag("--long")]
    public virtual bool? Long { get; set; }

    [CliFlag("--parseable")]
    public virtual bool? Parseable { get; set; }

    [CliFlag("--global")]
    public virtual bool? Global { get; set; }

    [CliOption("--depth")]
    public virtual int? Depth { get; set; }

    [CliOption("--omit")]
    public virtual string? Omit { get; set; }

    [CliOption("--include")]
    public virtual string? Include { get; set; }

    [CliFlag("--link")]
    public virtual bool? Link { get; set; }

    [CliFlag("--package-lock-only")]
    public virtual bool? PackageLockOnly { get; set; }

    [CliFlag("--unicode")]
    public virtual bool? Unicode { get; set; }

    [CliOption("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [CliFlag("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [CliFlag("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }

    [CliFlag("--install-links")]
    public virtual bool? InstallLinks { get; set; }
}