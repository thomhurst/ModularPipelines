using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ls")]
public record NpmLsOptions : NpmOptions
{
    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }

    [BooleanCommandSwitch("--long")]
    public virtual bool? Long { get; set; }

    [BooleanCommandSwitch("--parseable")]
    public virtual bool? Parseable { get; set; }

    [BooleanCommandSwitch("--global")]
    public virtual bool? Global { get; set; }

    [CommandSwitch("--depth")]
    public virtual int? Depth { get; set; }

    [CommandSwitch("--omit")]
    public virtual string? Omit { get; set; }

    [CommandSwitch("--include")]
    public virtual string? Include { get; set; }

    [BooleanCommandSwitch("--link")]
    public virtual bool? Link { get; set; }

    [BooleanCommandSwitch("--package-lock-only")]
    public virtual bool? PackageLockOnly { get; set; }

    [BooleanCommandSwitch("--unicode")]
    public virtual bool? Unicode { get; set; }

    [CommandSwitch("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }

    [BooleanCommandSwitch("--install-links")]
    public virtual bool? InstallLinks { get; set; }
}