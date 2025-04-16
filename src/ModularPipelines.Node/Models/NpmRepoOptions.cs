using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repo")]
public record NpmRepoOptions : NpmOptions
{
    [CommandSwitch("--browser")]
    public virtual string? Browser { get; set; }

    [CommandSwitch("--registry")]
    public virtual Uri? Registry { get; set; }

    [CommandSwitch("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Pkgname { get; set; }
}