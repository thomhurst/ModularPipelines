using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("cp")]
public record DockerCpOptions : DockerOptions
{
    [CommandLongSwitch("archive")]
    public string? Archive { get; set; }

    [CommandLongSwitch("follow-link")]
    public string? FollowLink { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}