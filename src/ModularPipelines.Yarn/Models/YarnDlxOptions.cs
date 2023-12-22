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
    public string? Package { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }
}