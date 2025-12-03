using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "docker", "images", "delete")]
public record GcloudArtifactsDockerImagesDeleteOptions(
[property: CliArgument] string Image
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--delete-tags")]
    public bool? DeleteTags { get; set; }
}