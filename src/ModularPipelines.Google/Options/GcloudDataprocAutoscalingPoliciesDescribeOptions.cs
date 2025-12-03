using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "autoscaling-policies", "describe")]
public record GcloudDataprocAutoscalingPoliciesDescribeOptions(
[property: CliArgument] string AutoscalingPolicy,
[property: CliArgument] string Region
) : GcloudOptions;