using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "topics", "get-iam-policy")]
public record GcloudPubsubTopicsGetIamPolicyOptions(
[property: PositionalArgument] string Topic
) : GcloudOptions;