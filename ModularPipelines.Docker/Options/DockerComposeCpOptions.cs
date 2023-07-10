using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose cp")]
public record DockerComposeCpOptions : DockerOptions
{
    [CommandLongSwitch("archive")]
    public string? Archive { get; set; }

    [CommandLongSwitch("follow-link")]
    public string? FollowLink { get; set; }

    [CommandLongSwitch("index")]
    public string? Index { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}