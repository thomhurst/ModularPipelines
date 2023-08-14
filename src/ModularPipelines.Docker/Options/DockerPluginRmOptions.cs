using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin rm")]
public record DockerPluginRmOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Plugins) : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}