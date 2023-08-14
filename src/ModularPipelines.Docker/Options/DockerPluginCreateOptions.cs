using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin create")]
public record DockerPluginCreateOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Plugin, [property: PositionalArgument(Position = Position.AfterArguments)] string PluginDataDirectory) : DockerOptions
{
    [BooleanCommandSwitch("--compress")]
    public bool? Compress { get; set; }
}