using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "autoscaling-policies", "describe")]
public record GcloudDataprocAutoscalingPoliciesDescribeOptions(
[property: PositionalArgument] string AutoscalingPolicy,
[property: PositionalArgument] string Region
) : GcloudOptions;