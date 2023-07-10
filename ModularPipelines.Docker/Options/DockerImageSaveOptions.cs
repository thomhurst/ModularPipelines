using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image save")]
public record DockerImageSaveOptions : DockerOptions
{
    [CommandLongSwitch("output")]
    public string? Output { get; set; }

}