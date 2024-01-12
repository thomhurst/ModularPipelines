using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "docker", "tags", "add")]
public record GcloudArtifactsDockerTagsAddOptions(
[property: PositionalArgument] string DockerImage,
[property: PositionalArgument] string DockerTag
) : GcloudOptions;