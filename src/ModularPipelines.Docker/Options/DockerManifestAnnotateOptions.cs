using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

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
    public string? Arch { get; set; }

    [CommandSwitch("--os")]
    public string? Os { get; set; }

    [CommandSwitch("--os-features")]
    public string? OsFeatures { get; set; }

    [CommandSwitch("--os-version")]
    public string? OsVersion { get; set; }

    [CommandSwitch("--variant")]
    public string? Variant { get; set; }
}
