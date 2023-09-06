using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("manifest create")]
[ExcludeFromCodeCoverage]
public record DockerManifestCreateOptions([property: PositionalArgument(Position = Position.AfterArguments)] string ManifestList, [property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Manifests) : DockerOptions
{
    [CommandSwitch("--amend")]
    public string? Amend { get; set; }

    [CommandSwitch("--insecure")]
    public string? Insecure { get; set; }
}
