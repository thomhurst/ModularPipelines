using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "get")]
public record NpmConfigGetOptions : NpmOptions
{
    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }

    [BooleanCommandSwitch("--global")]
    public virtual bool? Global { get; set; }

    [CommandSwitch("--editor")]
    public virtual string? Editor { get; set; }

    [CommandSwitch("--location")]
    public virtual string? Location { get; set; }

    [BooleanCommandSwitch("--long")]
    public virtual bool? Long { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Key { get; set; }
}