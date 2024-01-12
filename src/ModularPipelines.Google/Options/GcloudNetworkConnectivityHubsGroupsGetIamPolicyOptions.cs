using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "hubs", "groups", "get-iam-policy")]
public record GcloudNetworkConnectivityHubsGroupsGetIamPolicyOptions(
[property: PositionalArgument] string Group,
[property: PositionalArgument] string Hub
) : GcloudOptions;