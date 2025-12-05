using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "subscriptions", "get-iam-policy")]
public record GcloudPubsubSubscriptionsGetIamPolicyOptions(
[property: CliArgument] string Subscription
) : GcloudOptions;