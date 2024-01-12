using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "autoscaling-policies", "get-iam-policy")]
public record GcloudDataprocAutoscalingPoliciesGetIamPolicyOptions(
[property: PositionalArgument] string AutoscalingPolicy,
[property: PositionalArgument] string Region
) : GcloudOptions;