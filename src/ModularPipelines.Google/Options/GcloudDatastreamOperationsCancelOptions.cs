using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "operations", "cancel")]
public record GcloudDatastreamOperationsCancelOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location
) : GcloudOptions;