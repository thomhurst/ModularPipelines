using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "images", "add-labels")]
public record GcloudComputeImagesAddLabelsOptions(
[property: CliArgument] string ImageName,
[property: CliOption("--labels")] IEnumerable<KeyValue> Labels
) : GcloudOptions;