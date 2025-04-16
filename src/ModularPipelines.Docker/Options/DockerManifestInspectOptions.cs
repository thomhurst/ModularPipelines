using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerManifestInspectOptions : DockerOptions
{
    public DockerManifestInspectOptions(
        string manifest
    )
    {
        CommandParts = ["manifest", "inspect"];

        Manifest = manifest;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? ManifestList { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Manifest { get; set; }

    [CommandSwitch("--insecure")]
    public virtual string? Insecure { get; set; }

    [CommandSwitch("--verbose")]
    public virtual string? Verbose { get; set; }
}
