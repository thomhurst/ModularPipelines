using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust key generate")]
public record DockerTrustKeyGenerateOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Name) : DockerOptions
{
    [CommandSwitch("--dir")]
    public string? Dir { get; set; }

}