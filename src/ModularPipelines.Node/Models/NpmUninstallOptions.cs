using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("uninstall")]
public record NpmUninstallOptions : NpmOptions
{
    [CliFlag("--save")]
    public virtual bool? Save { get; set; }

    [CliFlag("--global")]
    public virtual bool? Global { get; set; }

    [CliOption("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [CliFlag("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [CliFlag("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }

    [CliFlag("--install-links")]
    public virtual bool? InstallLinks { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public virtual string? Pkg { get; set; }
}