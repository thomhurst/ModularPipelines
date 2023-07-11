using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("manifest push")]
public record DockerManifestPushOptions([property: PositionalArgument(Position = Position.AfterArguments)] string ManifestList) : DockerOptions
{

    [CommandSwitch("--insecure")]
    public string? Insecure { get; set; }

    [CommandSwitch("--purge")]
    public string? Purge { get; set; }

}