using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("manifest create")]
public record DockerManifestCreateOptions : DockerOptions
{
    [CommandLongSwitch("amend")]
    public string? Amend { get; set; }

    [CommandLongSwitch("insecure")]
    public string? Insecure { get; set; }

}