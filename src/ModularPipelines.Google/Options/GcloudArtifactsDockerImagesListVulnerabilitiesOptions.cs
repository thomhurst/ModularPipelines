using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "docker", "images", "list-vulnerabilities")]
public record GcloudArtifactsDockerImagesListVulnerabilitiesOptions(
[property: CliArgument] string Scan,
[property: CliArgument] string Location
) : GcloudOptions;