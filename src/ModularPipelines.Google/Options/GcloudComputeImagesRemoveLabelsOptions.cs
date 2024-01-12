using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "images", "remove-labels")]
public record GcloudComputeImagesRemoveLabelsOptions(
[property: PositionalArgument] string ImageName,
[property: BooleanCommandSwitch("--all")] bool All,
[property: CommandSwitch("--labels")] string[] Labels
) : GcloudOptions;