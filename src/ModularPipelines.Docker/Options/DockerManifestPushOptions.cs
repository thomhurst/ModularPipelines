using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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
    public virtual string? Insecure { get; set; }

    [CommandSwitch("--purge")]
    public virtual string? Purge { get; set; }
}
