using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "hubs", "groups", "list")]
public record GcloudNetworkConnectivityHubsGroupsListOptions(
[property: CommandSwitch("--hub")] string Hub
) : GcloudOptions;