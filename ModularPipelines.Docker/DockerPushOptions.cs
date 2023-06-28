using ModularPipelines.Docker.Options;

namespace ModularPipelines.Docker;

public record DockerPushOptions(string Name, string Tag) : DockerOptions
{
    public bool DisableContentTrust { get; init; } = true;
};