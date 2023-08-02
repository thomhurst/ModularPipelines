using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("attach")]
public record DockerAttachOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container) : DockerOptions
{
    [CommandSwitch("--no-stdin")]
    public string? NoStdin { get; set; }

    [BooleanCommandSwitch("--sig-proxy")]
    public bool? SigProxy { get; set; }

    [CommandSwitch("--detach-keys")]
    public string? DetachKeys { get; set; }
}