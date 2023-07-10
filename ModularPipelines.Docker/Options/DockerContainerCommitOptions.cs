using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container commit")]
public record DockerContainerCommitOptions : DockerOptions
{
    [CommandLongSwitch("author")]
    public string? Author { get; set; }

    [CommandLongSwitch("change")]
    public string? Change { get; set; }

    [CommandLongSwitch("message")]
    public string? Message { get; set; }

    [BooleanCommandSwitch("pause")]
    public bool? Pause { get; set; }

}