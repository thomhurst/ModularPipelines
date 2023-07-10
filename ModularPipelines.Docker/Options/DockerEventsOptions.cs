using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("events")]
public record DockerEventsOptions : DockerOptions
{
    [CommandLongSwitch("filter")]
    public string? Filter { get; set; }

    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [CommandLongSwitch("since")]
    public string? Since { get; set; }

    [CommandLongSwitch("until")]
    public string? Until { get; set; }

}