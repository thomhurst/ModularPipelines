using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "locations", "fetch-static-ips")]
public record GcloudDatastreamLocationsFetchStaticIpsOptions(
[property: PositionalArgument] string Location
) : GcloudOptions;