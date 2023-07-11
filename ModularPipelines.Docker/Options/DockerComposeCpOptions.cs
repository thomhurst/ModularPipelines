using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose cp")]
public record DockerComposeCpOptions([property: PositionalArgument(Position = Position.AfterArguments)] string SrcPath, [property: PositionalArgument(Position = Position.AfterArguments)] string DestPath) : DockerOptions
{

    [CommandSwitch("--archive")]
    public string? Archive { get; set; }

    [CommandSwitch("--follow-link")]
    public string? FollowLink { get; set; }

    [CommandSwitch("--index")]
    public string? Index { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

}