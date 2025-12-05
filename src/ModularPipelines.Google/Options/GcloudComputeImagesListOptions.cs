using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "images", "list")]
public record GcloudComputeImagesListOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliFlag("--preview-images")]
    public bool? PreviewImages { get; set; }

    [CliOption("--regexp")]
    public string? Regexp { get; set; }

    [CliFlag("--show-deprecated")]
    public bool? ShowDeprecated { get; set; }

    [CliFlag("--standard-images")]
    public bool? StandardImages { get; set; }
}