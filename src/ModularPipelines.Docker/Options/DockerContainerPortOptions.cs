using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container", "port")]
[ExcludeFromCodeCoverage]
public record DockerContainerPortOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? PrivatePortOrProto { get; set; }
}
