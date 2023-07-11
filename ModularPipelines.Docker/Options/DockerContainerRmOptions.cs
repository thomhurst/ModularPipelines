using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container rm")]
public record DockerContainerRmOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Container) : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }


    [CommandSwitch("--link")]
    public string? Link { get; set; }


    [CommandSwitch("--volumes")]
    public string? Volumes { get; set; }

}