using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "instances", "get-iam-policy")]
public record GcloudSpannerInstancesGetIamPolicyOptions(
[property: CliArgument] string Instance
) : GcloudOptions;