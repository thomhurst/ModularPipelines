using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "topics", "get-iam-policy")]
public record GcloudPubsubTopicsGetIamPolicyOptions(
[property: CliArgument] string Topic
) : GcloudOptions;