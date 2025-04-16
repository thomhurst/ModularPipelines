using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "delete")]
public record NpmConfigDeleteOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Key
) : NpmOptions
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
}