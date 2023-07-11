using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin enable")]
public record DockerPluginEnableOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Plugin) : DockerOptions
{

    [CommandSwitch("--timeout")]
    public int? Timeout { get; set; }

}