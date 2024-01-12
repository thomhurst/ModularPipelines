using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "images", "list")]
public record GcloudComputeImagesListOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [BooleanCommandSwitch("--preview-images")]
    public bool? PreviewImages { get; set; }

    [CommandSwitch("--regexp")]
    public string? Regexp { get; set; }

    [BooleanCommandSwitch("--show-deprecated")]
    public bool? ShowDeprecated { get; set; }

    [BooleanCommandSwitch("--standard-images")]
    public bool? StandardImages { get; set; }
}