using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("manifest annotate")]
[ExcludeFromCodeCoverage]
public record DockerManifestAnnotateOptions([property: PositionalArgument(Position = Position.AfterArguments)] string ManifestList, [property: PositionalArgument(Position = Position.AfterArguments)] string Manifest) : DockerOptions
{
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
