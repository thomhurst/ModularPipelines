using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("query")]
public record NpmQueryOptions : NpmOptions
{
    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--workspace")]
    public string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public bool? IncludeWorkspaceRoot { get; set; }

    [BooleanCommandSwitch("--package-lock-only")]
    public bool? PackageLockOnly { get; set; }
}