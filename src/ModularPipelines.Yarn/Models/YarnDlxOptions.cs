using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dlx")]
public record YarnDlxOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Command
) : YarnOptions
{
    [CommandSwitch("--package")]
    public virtual string? Package { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }
}