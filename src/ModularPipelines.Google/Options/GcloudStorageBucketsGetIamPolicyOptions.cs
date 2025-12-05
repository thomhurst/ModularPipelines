using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "buckets", "get-iam-policy")]
public record GcloudStorageBucketsGetIamPolicyOptions(
[property: CliArgument] string Url
) : GcloudOptions;