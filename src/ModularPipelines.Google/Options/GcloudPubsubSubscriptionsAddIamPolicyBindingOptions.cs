using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "subscriptions", "add-iam-policy-binding")]
public record GcloudPubsubSubscriptionsAddIamPolicyBindingOptions(
[property: CliArgument] string Subscription,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;