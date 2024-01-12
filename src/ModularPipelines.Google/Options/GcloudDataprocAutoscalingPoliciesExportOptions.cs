using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "autoscaling-policies", "export")]
public record GcloudDataprocAutoscalingPoliciesExportOptions(
[property: PositionalArgument] string AutoscalingPolicy,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }
}