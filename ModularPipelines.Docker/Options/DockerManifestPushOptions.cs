using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("manifest push")]
public record DockerManifestPushOptions : DockerOptions
{
    [CommandLongSwitch("insecure")]
    public string? Insecure { get; set; }

    [CommandLongSwitch("purge")]
    public string? Purge { get; set; }

}