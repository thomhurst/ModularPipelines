using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "docker", "images", "describe")]
public record GcloudArtifactsDockerImagesDescribeOptions(
[property: CliArgument] string Image
) : GcloudOptions
{
    [CliOption("--metadata-filter")]
    public string? MetadataFilter { get; set; }

    [CliFlag("--show-all-metadata")]
    public bool? ShowAllMetadata { get; set; }

    [CliFlag("--show-build-details")]
    public bool? ShowBuildDetails { get; set; }

    [CliFlag("--show-deployment")]
    public bool? ShowDeployment { get; set; }

    [CliFlag("--show-image-basis")]
    public bool? ShowImageBasis { get; set; }

    [CliFlag("--show-package-vulnerability")]
    public bool? ShowPackageVulnerability { get; set; }

    [CliFlag("--show-provenance")]
    public bool? ShowProvenance { get; set; }

    [CliFlag("--show-sbom-references")]
    public bool? ShowSbomReferences { get; set; }
}