using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "buckets", "get-iam-policy")]
public record GcloudStorageBucketsGetIamPolicyOptions(
[property: PositionalArgument] string Url
) : GcloudOptions;