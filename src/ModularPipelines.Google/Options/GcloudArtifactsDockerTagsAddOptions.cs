using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "docker", "tags", "add")]
public record GcloudArtifactsDockerTagsAddOptions(
[property: CliArgument] string DockerImage,
[property: CliArgument] string DockerTag
) : GcloudOptions;