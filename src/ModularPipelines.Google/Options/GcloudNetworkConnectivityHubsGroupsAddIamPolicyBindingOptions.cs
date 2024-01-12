using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "hubs", "groups", "add-iam-policy-binding")]
public record GcloudNetworkConnectivityHubsGroupsAddIamPolicyBindingOptions(
[property: PositionalArgument] string Group,
[property: PositionalArgument] string Hub,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;