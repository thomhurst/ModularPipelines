using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container cp")]
public record DockerContainerCpOptions([property: PositionalArgument(Position = Position.AfterArguments)] string SrcPath, [property: PositionalArgument(Position = Position.AfterArguments)] string DestPath) : DockerOptions
{

    [CommandSwitch("--archive")]
    public string? Archive { get; set; }


    [CommandSwitch("--follow-link")]
    public string? FollowLink { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

}