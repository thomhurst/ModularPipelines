using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin rm")]
public record DockerPluginRmOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Plugin) : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

}