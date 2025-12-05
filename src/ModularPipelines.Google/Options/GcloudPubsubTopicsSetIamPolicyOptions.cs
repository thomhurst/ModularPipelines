using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "topics", "set-iam-policy")]
public record GcloudPubsubTopicsSetIamPolicyOptions(
[property: CliArgument] string Topic,
[property: CliArgument] string PolicyFile
) : GcloudOptions;