using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "locations", "describe")]
public record GcloudNetworkConnectivityLocationsDescribeOptions(
[property: PositionalArgument] string Location
) : GcloudOptions;