using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("manifest inspect")]
public record DockerManifestInspectOptions : DockerOptions
{
    [CommandLongSwitch("insecure")]
    public string? Insecure { get; set; }

    [CommandLongSwitch("verbose")]
    public string? Verbose { get; set; }

}