using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("init")]
public record NpmInitOptions : NpmOptions
{
    [CommandSwitch("--init-author-name")]
    public string? InitAuthorName { get; set; }

    [CommandSwitch("--init-author-url")]
    public string? InitAuthorUrl { get; set; }

    [CommandSwitch("--init-license")]
    public string? InitLicense { get; set; }

    [CommandSwitch("--init-module")]
    public string? InitModule { get; set; }

    [CommandSwitch("--init-version")]
    public string? InitVersion { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--workspace")]
    public string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--workspaces-update")]
    public bool? WorkspacesUpdate { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public bool? IncludeWorkspaceRoot { get; set; }
}