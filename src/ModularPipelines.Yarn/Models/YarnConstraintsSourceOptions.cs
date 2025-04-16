using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("constraints", "source")]
public record YarnConstraintsSourceOptions : YarnOptions
{
    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }
}