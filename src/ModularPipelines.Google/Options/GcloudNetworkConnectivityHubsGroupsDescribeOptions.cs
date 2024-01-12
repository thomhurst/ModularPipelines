using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "hubs", "groups", "describe")]
public record GcloudNetworkConnectivityHubsGroupsDescribeOptions(
[property: PositionalArgument] string Group,
[property: PositionalArgument] string Hub
) : GcloudOptions;