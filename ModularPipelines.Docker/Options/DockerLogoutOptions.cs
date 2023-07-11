using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("logout")]
public record DockerLogoutOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string Server { get; set; }
}