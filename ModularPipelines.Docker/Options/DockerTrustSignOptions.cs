using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust sign")]
public record DockerTrustSignOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Image) : DockerOptions
{
    [CommandSwitch("--local")]
    public string? Local { get; set; }
}