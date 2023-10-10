using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ls")]
public record NpmLsOptions : NpmOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [BooleanCommandSwitch("--long")]
    public bool? Long { get; set; }

    [BooleanCommandSwitch("--parseable")]
    public bool? Parseable { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [BooleanCommandSwitch("--depth")]
    public bool? Depth { get; set; }

    [CommandSwitch("--omit")]
    public string? Omit { get; set; }

    [CommandSwitch("--include")]
    public string? Include { get; set; }

    [BooleanCommandSwitch("--link")]
    public bool? Link { get; set; }

    [BooleanCommandSwitch("--package-lock-only")]
    public bool? PackageLockOnly { get; set; }

    [BooleanCommandSwitch("--unicode")]
    public bool? Unicode { get; set; }

    [CommandSwitch("--workspace")]
    public string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public bool? IncludeWorkspaceRoot { get; set; }

    [BooleanCommandSwitch("--install-links")]
    public bool? InstallLinks { get; set; }
}