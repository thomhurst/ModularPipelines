using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "subscriptions", "remove-iam-policy-binding")]
public record GcloudPubsubSubscriptionsRemoveIamPolicyBindingOptions(
[property: PositionalArgument] string Subscription,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;