using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network inspect")]
public record DockerNetworkInspectOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Network) : DockerOptions
{

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--verbose")]
    public string? Verbose { get; set; }

}