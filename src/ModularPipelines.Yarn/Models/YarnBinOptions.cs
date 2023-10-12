using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bin")]
public record YarnBinOptions : YarnOptions
{
    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Name { get; set; }
}