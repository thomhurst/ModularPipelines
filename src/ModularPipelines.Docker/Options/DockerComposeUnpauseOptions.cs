using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose unpause")]
[ExcludeFromCodeCoverage]
public record DockerComposeUnpauseOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public IEnumerable<string>? Service { get; set; }
}
