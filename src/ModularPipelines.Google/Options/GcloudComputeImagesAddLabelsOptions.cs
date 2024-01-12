using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "images", "add-labels")]
public record GcloudComputeImagesAddLabelsOptions(
[property: PositionalArgument] string ImageName,
[property: CommandSwitch("--labels")] IEnumerable<KeyValue> Labels
) : GcloudOptions;