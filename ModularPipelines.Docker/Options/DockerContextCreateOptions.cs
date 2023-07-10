using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context create")]
public record DockerContextCreateOptions : DockerOptions
{
    [CommandLongSwitch("description")]
    public string? Description { get; set; }

}