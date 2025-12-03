using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "operations", "delete")]
public record GcloudDatastreamOperationsDeleteOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location
) : GcloudOptions;