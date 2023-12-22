using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("manifest push")]
[ExcludeFromCodeCoverage]
public record DockerManifestPushOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string ManifestList) : DockerOptions
{
    [CommandSwitch("--insecure")]
    public string? Insecure { get; set; }

    [CommandSwitch("--purge")]
    public string? Purge { get; set; }
}