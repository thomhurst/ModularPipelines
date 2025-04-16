using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rebuild")]
public record NpmRebuildOptions : NpmOptions
{
    [BooleanCommandSwitch("--global")]
    public virtual bool? Global { get; set; }

    [BooleanCommandSwitch("--bin-links")]
    public virtual bool? BinLinks { get; set; }

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
}