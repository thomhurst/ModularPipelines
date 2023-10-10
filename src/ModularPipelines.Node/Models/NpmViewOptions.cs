using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("view")]
public record NpmViewOptions : NpmOptions
{
    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [CommandSwitch("--workspace")]
    public string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public bool? IncludeWorkspaceRoot { get; set; }
}