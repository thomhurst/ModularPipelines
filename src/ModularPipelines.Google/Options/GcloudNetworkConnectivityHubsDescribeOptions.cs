using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs", "describe")]
public record GcloudNetworkConnectivityHubsDescribeOptions(
[property: CliArgument] string Hub
) : GcloudOptions;