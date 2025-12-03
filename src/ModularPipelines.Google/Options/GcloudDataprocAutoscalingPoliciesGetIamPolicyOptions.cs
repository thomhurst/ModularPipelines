using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "autoscaling-policies", "get-iam-policy")]
public record GcloudDataprocAutoscalingPoliciesGetIamPolicyOptions(
[property: CliArgument] string AutoscalingPolicy,
[property: CliArgument] string Region
) : GcloudOptions;