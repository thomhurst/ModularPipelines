using ModularPipelines.Attributes;
using ModularPipelines.FileSystem;
using ModularPipelines.Options;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Docker.Options;

public record DockerBuildOptions(Folder DockerfileFolder) : CommandLineToolOptions("docker")
{
    public string? Name { get; init; }
    
    [CommandSwitch("t")]
    public string? Tag { get; init; }

    [CommandLongSwitch("build-arg", SwitchValueSeparator = " ")]
    public IEnumerable<string>? BuildArguments { get; init; }
    
    public string? Dockerfile { get; init; }
}