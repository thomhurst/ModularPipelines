using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin create")]
public record DockerPluginCreateOptions : DockerOptions
{
    [CommandLongSwitch("compress")]
    public string? Compress { get; set; }

}