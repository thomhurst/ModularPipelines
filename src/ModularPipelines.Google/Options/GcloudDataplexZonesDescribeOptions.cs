using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "zones", "describe")]
public record GcloudDataplexZonesDescribeOptions(
[property: PositionalArgument] string Zone,
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location
) : GcloudOptions;