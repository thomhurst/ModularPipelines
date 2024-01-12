using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "docker", "images", "scan")]
public record GcloudArtifactsDockerImagesScanOptions(
[property: PositionalArgument] string ResourceUri
) : GcloudOptions
{
    [CommandSwitch("--additional-package-types")]
    public string[]? AdditionalPackageTypes { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--remote")]
    public bool? Remote { get; set; }
}