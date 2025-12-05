using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs", "groups", "get-iam-policy")]
public record GcloudNetworkConnectivityHubsGroupsGetIamPolicyOptions(
[property: CliArgument] string Group,
[property: CliArgument] string Hub
) : GcloudOptions;