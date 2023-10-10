using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run-script")]
public record NpmRunScriptOptions : NpmOptions
{
    [CommandSwitch("--workspace")]
    public string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public bool? IncludeWorkspaceRoot { get; set; }

    [BooleanCommandSwitch("--if-present")]
    public bool? IfPresent { get; set; }

    [BooleanCommandSwitch("--ignore-scripts")]
    public bool? IgnoreScripts { get; set; }

    [BooleanCommandSwitch("--foreground-scripts")]
    public bool? ForegroundScripts { get; set; }

    [CommandSwitch("--script-shell")]
    public string? ScriptShell { get; set; }
}