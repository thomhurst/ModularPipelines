using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("exec", "--")]
public record NpmExecOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Value,
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Cmd
) : NpmOptions
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
    public string? Pkg { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Version { get; set; }
}