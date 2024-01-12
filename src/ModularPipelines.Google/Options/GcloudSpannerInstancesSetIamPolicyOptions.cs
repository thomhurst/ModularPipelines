using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "instances", "set-iam-policy")]
public record GcloudSpannerInstancesSetIamPolicyOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;