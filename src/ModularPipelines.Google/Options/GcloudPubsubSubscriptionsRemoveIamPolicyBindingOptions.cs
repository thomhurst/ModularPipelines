using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "subscriptions", "remove-iam-policy-binding")]
public record GcloudPubsubSubscriptionsRemoveIamPolicyBindingOptions(
[property: CliArgument] string Subscription,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;