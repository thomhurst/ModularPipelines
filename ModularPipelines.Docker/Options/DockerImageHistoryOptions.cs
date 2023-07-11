using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image history")]
public record DockerImageHistoryOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Image) : DockerOptions
{
    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [BooleanCommandSwitch("--human")]
    public bool? Human { get; set; }

    [BooleanCommandSwitch("--no-trunc")]
    public bool? NoTrunc { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

}