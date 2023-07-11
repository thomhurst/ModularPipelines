using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("cp")]
public record DockerCpOptions([property: PositionalArgument(Position = Position.AfterArguments)] string SrcPath, [property: PositionalArgument(Position = Position.AfterArguments)] string DestPath) : DockerOptions
{

    [CommandSwitch("--archive")]
    public string? Archive { get; set; }


    [CommandSwitch("--follow-link")]
    public string? FollowLink { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

}