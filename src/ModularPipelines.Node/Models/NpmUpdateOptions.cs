using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("update")]
public record NpmUpdateOptions : NpmOptions
{
    [BooleanCommandSwitch("--save")]
    public bool? Save { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--install-strategy")]
    public string? InstallStrategy { get; set; }

    [CommandSwitch("--omit")]
    public string? Omit { get; set; }

    [CommandSwitch("--include")]
    public string? Include { get; set; }

    [BooleanCommandSwitch("--strict-peer-deps")]
    public bool? StrictPeerDeps { get; set; }

    [BooleanCommandSwitch("--package-lock")]
    public bool? PackageLock { get; set; }

    [BooleanCommandSwitch("--foreground-scripts")]
    public bool? ForegroundScripts { get; set; }

    [BooleanCommandSwitch("--ignore-scripts")]
    public bool? IgnoreScripts { get; set; }

    [BooleanCommandSwitch("--audit")]
    public bool? Audit { get; set; }

    [BooleanCommandSwitch("--bin-links")]
    public bool? BinLinks { get; set; }

    [BooleanCommandSwitch("--fund")]
    public bool? Fund { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [CommandSwitch("--workspace")]
    public string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public bool? IncludeWorkspaceRoot { get; set; }

    [BooleanCommandSwitch("--install-links")]
    public bool? InstallLinks { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Pkg { get; set; }
}