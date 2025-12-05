using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs", "groups", "list")]
public record GcloudNetworkConnectivityHubsGroupsListOptions(
[property: CliOption("--hub")] string Hub
) : GcloudOptions;