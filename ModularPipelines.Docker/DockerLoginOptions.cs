using ModularPipelines.Docker.Options;

namespace ModularPipelines.Docker;

public record DockerLoginOptions(string Username, string Password) : DockerOptions
{
    public Uri? Server { get; init; }
}