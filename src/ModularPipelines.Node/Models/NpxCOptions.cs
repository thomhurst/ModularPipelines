using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("npx", "-c")]
public record NpxCOptions : NpmOptions
{
    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Cmd { get; set; }
}