using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerManifestAnnotateOptions : DockerOptions
{
    public DockerManifestAnnotateOptions(
        string manifestList,
        string manifest
    )
    {
        CommandParts = ["manifest", "annotate"];

        ManifestList = manifestList;

        Manifest = manifest;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? ManifestList { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Manifest { get; set; }

    [CliOption("--arch")]
    public virtual string? Arch { get; set; }

    [CliOption("--os")]
    public virtual string? Os { get; set; }

    [CliOption("--os-features")]
    public virtual string? OsFeatures { get; set; }

    [CliOption("--os-version")]
    public virtual string? OsVersion { get; set; }

    [CliOption("--variant")]
    public virtual string? Variant { get; set; }
}
