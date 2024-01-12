using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "subscriptions", "set-iam-policy")]
public record GcloudPubsubSubscriptionsSetIamPolicyOptions(
[property: PositionalArgument] string Subscription,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;