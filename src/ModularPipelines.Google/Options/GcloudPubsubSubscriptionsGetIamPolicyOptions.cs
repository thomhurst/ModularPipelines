using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "subscriptions", "get-iam-policy")]
public record GcloudPubsubSubscriptionsGetIamPolicyOptions(
[property: PositionalArgument] string Subscription
) : GcloudOptions;