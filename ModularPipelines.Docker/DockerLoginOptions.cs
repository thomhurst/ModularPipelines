using ModularPipelines.Docker.Options;
using ModularPipelines.FileSystem;
using ModularPipelines.Options;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Docker;

public record DockerLoginOptions(string Username, string Password) : DockerOptions
{
    public Uri? Server { get; init; }
}

public record DockerBuildOptions(Folder DockerfileFolder) : CommandLineToolOptions("docker")
{
    public string? Name { get; init; }
    
    public string? Tag { get; init; }

    public IEnumerable<string>? BuildArguments { get; init; }
    
    public File? Dockerfile { get; init; }
}

public record DockerPushOptions(string Name, string Tag) : DockerOptions
{
    public bool DisableContentTrust { get; init; } = true;
};