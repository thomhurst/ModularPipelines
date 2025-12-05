using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "autoscaling-policies", "set-iam-policy")]
public record GcloudDataprocAutoscalingPoliciesSetIamPolicyOptions(
[property: CliArgument] string AutoscalingPolicy,
[property: CliArgument] string Region,
[property: CliArgument] string PolicyFile
) : GcloudOptions;