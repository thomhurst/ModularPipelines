using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "docker", "tags", "delete")]
public record GcloudArtifactsDockerTagsDeleteOptions(
[property: PositionalArgument] string DockerTag
) : GcloudOptions;