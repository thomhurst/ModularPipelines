using ModularPipelines.FileSystem;
using ModularPipelines.Options;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Docker;

public record DockerBuildOptions(Folder DockerfileFolder) : CommandLineToolOptions("docker")
{
    public string? Name { get; init; }
    
    public string? Tag { get; init; }

    public IEnumerable<string>? BuildArguments { get; init; }
    
    public File? Dockerfile { get; init; }
}