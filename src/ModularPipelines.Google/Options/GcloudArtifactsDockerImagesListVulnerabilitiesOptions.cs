using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "docker", "images", "list-vulnerabilities")]
public record GcloudArtifactsDockerImagesListVulnerabilitiesOptions(
[property: PositionalArgument] string Scan,
[property: PositionalArgument] string Location
) : GcloudOptions;