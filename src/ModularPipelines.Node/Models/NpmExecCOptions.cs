using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("exec", "-c")]
public record NpmExecCOptions : NpmOptions
{
    [CommandSwitch("--package")]
    public string[]? Package { get; set; }

    [CommandSwitch("--call")]
    public string? Call { get; set; }

    [CommandSwitch("--workspace")]
    public string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public bool? IncludeWorkspaceRoot { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Cmd { get; set; }
}