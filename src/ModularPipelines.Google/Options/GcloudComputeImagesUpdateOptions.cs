using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "images", "update")]
public record GcloudComputeImagesUpdateOptions(
[property: CliArgument] string ImageName
) : GcloudOptions
{
    [CliOption("--architecture")]
    public string? Architecture { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--family")]
    public string? Family { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}