using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "docker", "images", "scan")]
public record GcloudArtifactsDockerImagesScanOptions(
[property: CliArgument] string ResourceUri
) : GcloudOptions
{
    [CliOption("--additional-package-types")]
    public string[]? AdditionalPackageTypes { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--remote")]
    public bool? Remote { get; set; }
}