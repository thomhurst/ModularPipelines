using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "locations", "fetch-static-ips")]
public record GcloudDatastreamLocationsFetchStaticIpsOptions(
[property: CliArgument] string Location
) : GcloudOptions;