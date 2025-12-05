using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "images", "remove-labels")]
public record GcloudComputeImagesRemoveLabelsOptions(
[property: CliArgument] string ImageName,
[property: CliFlag("--all")] bool All,
[property: CliOption("--labels")] string[] Labels
) : GcloudOptions;