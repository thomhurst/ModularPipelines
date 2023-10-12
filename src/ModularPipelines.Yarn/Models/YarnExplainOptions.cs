using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("explain")]
public record YarnExplainOptions : YarnOptions
{
    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Code { get; set; }
}