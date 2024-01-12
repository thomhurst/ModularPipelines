using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "hubs", "remove-iam-policy-binding")]
public record GcloudNetworkConnectivityHubsRemoveIamPolicyBindingOptions(
[property: PositionalArgument] string Hub,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;