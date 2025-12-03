using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "subscriptions", "set-iam-policy")]
public record GcloudPubsubSubscriptionsSetIamPolicyOptions(
[property: CliArgument] string Subscription,
[property: CliArgument] string PolicyFile
) : GcloudOptions;