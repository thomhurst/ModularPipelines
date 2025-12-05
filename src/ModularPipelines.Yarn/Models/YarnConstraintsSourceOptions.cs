using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("constraints", "source")]
public record YarnConstraintsSourceOptions : YarnOptions
{
    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }
}