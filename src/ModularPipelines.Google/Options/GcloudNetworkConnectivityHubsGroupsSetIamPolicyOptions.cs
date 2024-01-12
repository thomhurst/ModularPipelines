using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "hubs", "groups", "set-iam-policy")]
public record GcloudNetworkConnectivityHubsGroupsSetIamPolicyOptions(
[property: PositionalArgument] string Group,
[property: PositionalArgument] string Hub,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;