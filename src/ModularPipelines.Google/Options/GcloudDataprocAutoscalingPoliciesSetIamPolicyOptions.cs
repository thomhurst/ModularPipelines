using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "autoscaling-policies", "set-iam-policy")]
public record GcloudDataprocAutoscalingPoliciesSetIamPolicyOptions(
[property: PositionalArgument] string AutoscalingPolicy,
[property: PositionalArgument] string Region,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;