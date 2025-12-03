using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs", "groups", "describe")]
public record GcloudNetworkConnectivityHubsGroupsDescribeOptions(
[property: CliArgument] string Group,
[property: CliArgument] string Hub
) : GcloudOptions;