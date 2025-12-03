using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "locations", "list")]
public record GcloudNetworkConnectivityLocationsListOptions : GcloudOptions;