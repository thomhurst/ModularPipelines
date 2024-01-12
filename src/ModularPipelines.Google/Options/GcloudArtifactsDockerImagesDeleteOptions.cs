using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "docker", "images", "delete")]
public record GcloudArtifactsDockerImagesDeleteOptions(
[property: PositionalArgument] string Image
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--delete-tags")]
    public bool? DeleteTags { get; set; }
}