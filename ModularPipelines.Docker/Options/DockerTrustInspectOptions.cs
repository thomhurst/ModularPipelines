using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust inspect")]
public record DockerTrustInspectOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public IEnumerable<string> Images { get; set; }
    [CommandSwitch("--pretty")]
    public string? Pretty { get; set; }

}