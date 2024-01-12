using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "topics", "add-iam-policy-binding")]
public record GcloudPubsubTopicsAddIamPolicyBindingOptions(
[property: PositionalArgument] string Topic,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;