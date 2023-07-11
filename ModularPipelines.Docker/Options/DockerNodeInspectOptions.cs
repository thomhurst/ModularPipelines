using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("node inspect")]
public record DockerNodeInspectOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public IEnumerable<string> Node { get; set; }

    [CommandSwitch("--pretty")]
    public string? Pretty { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }
}