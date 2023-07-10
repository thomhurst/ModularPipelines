using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context update")]
public record DockerContextUpdateOptions : DockerOptions
{
    [CommandLongSwitch("description")]
    public string? Description { get; set; }

    [CommandLongSwitch("docker")]
    public string? Docker { get; set; }

}