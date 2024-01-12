using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "autoscaling-policies", "delete")]
public record GcloudDataprocAutoscalingPoliciesDeleteOptions(
[property: PositionalArgument] string AutoscalingPolicy,
[property: PositionalArgument] string Region
) : GcloudOptions;