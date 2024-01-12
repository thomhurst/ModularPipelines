using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "images", "delete")]
public record GcloudContainerImagesDeleteOptions(
[property: PositionalArgument] string ImageName
) : GcloudOptions
{
    [BooleanCommandSwitch("--force-delete-tags")]
    public bool? ForceDeleteTags { get; set; }
}