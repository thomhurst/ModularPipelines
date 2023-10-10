using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pack")]
public record NpmPackOptions : NpmOptions
{
    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [CommandSwitch("--pack-destination")]
    public string? PackDestination { get; set; }

    [CommandSwitch("--workspace")]
    public string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public bool? IncludeWorkspaceRoot { get; set; }
}