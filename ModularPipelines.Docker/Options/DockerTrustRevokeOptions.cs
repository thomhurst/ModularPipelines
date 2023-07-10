using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust revoke")]
public record DockerTrustRevokeOptions : DockerOptions
{
    [CommandLongSwitch("yes")]
    public string? Yes { get; set; }

}