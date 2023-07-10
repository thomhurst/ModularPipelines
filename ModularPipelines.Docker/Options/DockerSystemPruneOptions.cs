using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("system prune")]
public record DockerSystemPruneOptions : DockerOptions
{
    [CommandLongSwitch("all")]
    public string? All { get; set; }

    [CommandLongSwitch("force")]
    public string? Force { get; set; }

    [CommandLongSwitch("volumes")]
    public string? Volumes { get; set; }

}