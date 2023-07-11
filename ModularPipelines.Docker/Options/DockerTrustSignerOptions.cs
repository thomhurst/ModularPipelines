using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust signer")]
public record DockerTrustSignerOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}