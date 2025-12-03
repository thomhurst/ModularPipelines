using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "images", "delete")]
public record GcloudContainerImagesDeleteOptions(
[property: CliArgument] string ImageName
) : GcloudOptions
{
    [CliFlag("--force-delete-tags")]
    public bool? ForceDeleteTags { get; set; }
}