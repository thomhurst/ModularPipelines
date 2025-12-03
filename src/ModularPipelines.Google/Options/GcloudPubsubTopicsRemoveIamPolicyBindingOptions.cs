using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "topics", "remove-iam-policy-binding")]
public record GcloudPubsubTopicsRemoveIamPolicyBindingOptions(
[property: CliArgument] string Topic,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;