using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs", "groups", "set-iam-policy")]
public record GcloudNetworkConnectivityHubsGroupsSetIamPolicyOptions(
[property: CliArgument] string Group,
[property: CliArgument] string Hub,
[property: CliArgument] string PolicyFile
) : GcloudOptions;