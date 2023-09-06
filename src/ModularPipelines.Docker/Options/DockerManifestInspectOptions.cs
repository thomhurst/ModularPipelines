using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("manifest inspect")]
[ExcludeFromCodeCoverage]
public record DockerManifestInspectOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Manifest) : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string? ManifestList { get; set; }

    [CommandSwitch("--insecure")]
    public string? Insecure { get; set; }

    [CommandSwitch("--verbose")]
    public string? Verbose { get; set; }
}
