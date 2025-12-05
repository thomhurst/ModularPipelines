using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "docker", "tags", "delete")]
public record GcloudArtifactsDockerTagsDeleteOptions(
[property: CliArgument] string DockerTag
) : GcloudOptions;