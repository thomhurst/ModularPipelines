using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "topics", "set-iam-policy")]
public record GcloudPubsubTopicsSetIamPolicyOptions(
[property: PositionalArgument] string Topic,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;