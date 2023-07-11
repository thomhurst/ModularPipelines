using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust revoke")]
public record DockerTrustRevokeOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string Image { get; set; }
    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }

}