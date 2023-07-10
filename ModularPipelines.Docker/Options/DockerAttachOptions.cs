using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("attach")]
public record DockerAttachOptions : DockerOptions
{
    [CommandLongSwitch("no-stdin")]
    public string? NoStdin { get; set; }

    [BooleanCommandSwitch("sig-proxy")]
    public bool? SigProxy { get; set; }

}