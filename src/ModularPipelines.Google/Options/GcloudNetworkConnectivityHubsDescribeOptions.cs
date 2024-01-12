using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "hubs", "describe")]
public record GcloudNetworkConnectivityHubsDescribeOptions(
[property: PositionalArgument] string Hub
) : GcloudOptions;