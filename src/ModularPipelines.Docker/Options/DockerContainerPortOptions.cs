using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container port")]
[ExcludeFromCodeCoverage]
public record DockerContainerPortOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container) : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string? PrivatePort { get; set; }
}
