using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container export")]
public record DockerContainerExportOptions : DockerOptions
{
    [CommandLongSwitch("output")]
    public string? Output { get; set; }

}