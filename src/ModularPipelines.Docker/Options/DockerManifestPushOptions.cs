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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? ManifestList { get; set; }

    [CliOption("--insecure")]
    public virtual string? Insecure { get; set; }

    [CliOption("--purge")]
    public virtual string? Purge { get; set; }
}
