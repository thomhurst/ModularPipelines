using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "docker", "images", "get-operation")]
public record GcloudArtifactsDockerImagesGetOperationOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location
) : GcloudOptions;