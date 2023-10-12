using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("constraints", "source")]
public record YarnConstraintsSourceOptions : YarnOptions
{
    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }
}