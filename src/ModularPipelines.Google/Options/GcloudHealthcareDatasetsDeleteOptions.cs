using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "datasets", "delete")]
public record GcloudHealthcareDatasetsDeleteOptions(
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location
) : GcloudOptions;