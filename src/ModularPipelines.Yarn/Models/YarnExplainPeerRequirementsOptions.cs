using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("explain", "peer-requirements")]
public record YarnExplainPeerRequirementsOptions : YarnOptions
{
    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Hash { get; set; }
}