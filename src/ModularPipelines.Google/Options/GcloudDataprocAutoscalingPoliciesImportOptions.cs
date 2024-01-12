using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "autoscaling-policies", "import")]
public record GcloudDataprocAutoscalingPoliciesImportOptions(
[property: PositionalArgument] string AutoscalingPolicy,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--source")]
    public string? Source { get; set; }
}