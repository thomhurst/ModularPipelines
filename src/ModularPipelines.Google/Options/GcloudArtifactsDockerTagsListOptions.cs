using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "docker", "tags", "list")]
public record GcloudArtifactsDockerTagsListOptions(
[property: CliArgument] string ImagePath
) : GcloudOptions;