using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pkg", "delete")]
public record NpmPkgDeleteOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Key
) : NpmOptions
{
    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }

    [CommandSwitch("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public virtual bool? Workspaces { get; set; }
}