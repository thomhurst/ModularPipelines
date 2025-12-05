using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "zones", "describe")]
public record GcloudDataplexZonesDescribeOptions(
[property: CliArgument] string Zone,
[property: CliArgument] string Lake,
[property: CliArgument] string Location
) : GcloudOptions;