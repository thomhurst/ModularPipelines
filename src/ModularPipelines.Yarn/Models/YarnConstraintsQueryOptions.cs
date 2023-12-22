using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("constraints", "query")]
public record YarnConstraintsQueryOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Query
) : YarnOptions
{
    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }
}