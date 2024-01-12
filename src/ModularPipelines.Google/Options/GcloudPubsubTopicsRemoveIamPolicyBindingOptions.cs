using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "topics", "remove-iam-policy-binding")]
public record GcloudPubsubTopicsRemoveIamPolicyBindingOptions(
[property: PositionalArgument] string Topic,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;