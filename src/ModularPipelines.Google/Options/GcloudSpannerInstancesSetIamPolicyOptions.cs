using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "instances", "set-iam-policy")]
public record GcloudSpannerInstancesSetIamPolicyOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string PolicyFile
) : GcloudOptions;