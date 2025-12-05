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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? ManifestList { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Manifest { get; set; }

    [CliOption("--insecure")]
    public virtual string? Insecure { get; set; }

    [CliOption("--verbose")]
    public virtual string? Verbose { get; set; }
}
