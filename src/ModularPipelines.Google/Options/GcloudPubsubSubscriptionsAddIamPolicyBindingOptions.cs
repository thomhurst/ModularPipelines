using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "subscriptions", "add-iam-policy-binding")]
public record GcloudPubsubSubscriptionsAddIamPolicyBindingOptions(
[property: PositionalArgument] string Subscription,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;