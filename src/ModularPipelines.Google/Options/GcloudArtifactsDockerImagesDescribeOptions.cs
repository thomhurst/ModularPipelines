using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "docker", "images", "describe")]
public record GcloudArtifactsDockerImagesDescribeOptions(
[property: PositionalArgument] string Image
) : GcloudOptions
{
    [CommandSwitch("--metadata-filter")]
    public string? MetadataFilter { get; set; }

    [BooleanCommandSwitch("--show-all-metadata")]
    public bool? ShowAllMetadata { get; set; }

    [BooleanCommandSwitch("--show-build-details")]
    public bool? ShowBuildDetails { get; set; }

    [BooleanCommandSwitch("--show-deployment")]
    public bool? ShowDeployment { get; set; }

    [BooleanCommandSwitch("--show-image-basis")]
    public bool? ShowImageBasis { get; set; }

    [BooleanCommandSwitch("--show-package-vulnerability")]
    public bool? ShowPackageVulnerability { get; set; }

    [BooleanCommandSwitch("--show-provenance")]
    public bool? ShowProvenance { get; set; }

    [BooleanCommandSwitch("--show-sbom-references")]
    public bool? ShowSbomReferences { get; set; }
}