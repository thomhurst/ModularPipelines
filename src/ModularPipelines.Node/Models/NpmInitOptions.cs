using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("init", "(same", "as", "`npx")]
public record NpmInitOptions
(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Value
) : NpmOptions
{
    [CommandSwitch("--init-author-name")]
    public virtual string? InitAuthorName { get; set; }

    [CommandSwitch("--init-author-url")]
    public virtual string? InitAuthorUrl { get; set; }

    [CommandSwitch("--init-license")]
    public virtual string? InitLicense { get; set; }

    [CommandSwitch("--init-module")]
    public virtual string? InitModule { get; set; }

    [CommandSwitch("--init-version")]
    public virtual string? InitVersion { get; set; }

    [BooleanCommandSwitch("--yes")]
    public virtual bool? Yes { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [CommandSwitch("--scope")]
    public virtual string? Scope { get; set; }

    [CommandSwitch("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--workspaces-update")]
    public virtual bool? WorkspacesUpdate { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }
}