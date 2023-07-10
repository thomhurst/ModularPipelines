using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container attach")]
public record DockerContainerAttachOptions : DockerOptions
{
    [CommandLongSwitch("detach-keys")]
    public string? DetachKeys { get; set; }

    [CommandLongSwitch("no-stdin")]
    public string? NoStdin { get; set; }

    [BooleanCommandSwitch("sig-proxy")]
    public bool? SigProxy { get; set; }

}