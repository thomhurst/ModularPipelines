using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "environments", "describe")]
public record GcloudDataplexEnvironmentsDescribeOptions(
[property: PositionalArgument] string Environment,
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location
) : GcloudOptions;