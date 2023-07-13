using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("system events")]
public record DockerSystemEventsOptions : DockerOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--since")]
    public string? Since { get; set; }

    [CommandSwitch("--until")]
    public string? Until { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }
}