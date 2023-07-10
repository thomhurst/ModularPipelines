using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx du")]
public record DockerBuildxDuOptions : DockerOptions
{
    [CommandLongSwitch("filter")]
    public string? Filter { get; set; }

    [CommandLongSwitch("verbose")]
    public string? Verbose { get; set; }

}