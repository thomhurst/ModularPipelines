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

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? ManifestList { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Manifest { get; set; }

    [CommandSwitch("--arch")]
    public virtual string? Arch { get; set; }

    [CommandSwitch("--os")]
    public virtual string? Os { get; set; }

    [CommandSwitch("--os-features")]
    public virtual string? OsFeatures { get; set; }

    [CommandSwitch("--os-version")]
    public virtual string? OsVersion { get; set; }

    [CommandSwitch("--variant")]
    public virtual string? Variant { get; set; }
}
