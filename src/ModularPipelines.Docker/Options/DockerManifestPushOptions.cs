using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerManifestPushOptions : DockerOptions
{
    public DockerManifestPushOptions(
        string manifestList
    )
    {
        CommandParts = ["manifest", "push"];

        ManifestList = manifestList;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? ManifestList { get; set; }

    [CommandSwitch("--insecure")]
    public string? Insecure { get; set; }

    [CommandSwitch("--purge")]
    public string? Purge { get; set; }
}
