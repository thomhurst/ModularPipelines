using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin disable")]
public record DockerPluginDisableOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Plugin) : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

}