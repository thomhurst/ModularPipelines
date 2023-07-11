using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("system prune")]
public record DockerSystemPruneOptions : DockerOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--volumes")]
    public string? Volumes { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }
}