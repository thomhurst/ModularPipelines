using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container attach")]
[ExcludeFromCodeCoverage]
public record DockerContainerAttachOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container) : DockerOptions
{
    [CommandSwitch("--detach-keys")]
    public string? DetachKeys { get; set; }

    [CommandSwitch("--no-stdin")]
    public string? NoStdin { get; set; }

    [BooleanCommandSwitch("--sig-proxy")]
    public bool? SigProxy { get; set; }
}
