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
    public virtual string[]? Package { get; set; }

    [CommandSwitch("--call")]
    public virtual string? Call { get; set; }

    [CommandSwitch("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Pkg { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Version { get; set; }
}