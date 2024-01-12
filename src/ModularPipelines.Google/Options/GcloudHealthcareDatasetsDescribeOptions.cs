using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "datasets", "describe")]
public record GcloudHealthcareDatasetsDescribeOptions(
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location
) : GcloudOptions;