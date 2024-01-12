using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "operations", "delete")]
public record GcloudDatastreamOperationsDeleteOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Location
) : GcloudOptions;