using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "lakes", "describe")]
public record GcloudDataplexLakesDescribeOptions(
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location
) : GcloudOptions;