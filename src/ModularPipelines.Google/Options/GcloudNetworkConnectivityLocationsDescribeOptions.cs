using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "locations", "describe")]
public record GcloudNetworkConnectivityLocationsDescribeOptions(
[property: CliArgument] string Location
) : GcloudOptions;