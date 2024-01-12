using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "instances", "get-iam-policy")]
public record GcloudSpannerInstancesGetIamPolicyOptions(
[property: PositionalArgument] string Instance
) : GcloudOptions;