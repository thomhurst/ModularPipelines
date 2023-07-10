using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("commit")]
public record DockerCommitOptions : DockerOptions
{
    [CommandLongSwitch("author")]
    public string? Author { get; set; }

    [CommandLongSwitch("message")]
    public string? Message { get; set; }

    [BooleanCommandSwitch("pause")]
    public bool? Pause { get; set; }

}