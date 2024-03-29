using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("node", "demote")]
[ExcludeFromCodeCoverage]
public record DockerNodeDemoteOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Node { get; set; }
}
