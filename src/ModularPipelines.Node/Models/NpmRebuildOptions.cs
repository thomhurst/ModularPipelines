using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("rebuild")]
public record NpmRebuildOptions : NpmOptions
{
    [CliFlag("--global")]
    public virtual bool? Global { get; set; }

    [CliFlag("--bin-links")]
    public virtual bool? BinLinks { get; set; }

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