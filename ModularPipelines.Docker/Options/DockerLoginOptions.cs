using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

public record DockerLoginOptions(
    [property: CommandLongSwitch("username")] string Username,
    [property: CommandLongSwitch("password")] string Password
) : DockerOptions
{
    public Uri? Server { get; init; }
}
