using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("system events")]
public record DockerSystemEventsOptions : DockerOptions
{
    [CommandLongSwitch("filter")]
    public string? Filter { get; set; }

    [CommandLongSwitch("since")]
    public string? Since { get; set; }

    [CommandLongSwitch("until")]
    public string? Until { get; set; }

}