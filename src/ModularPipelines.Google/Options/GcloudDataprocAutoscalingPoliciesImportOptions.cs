using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "autoscaling-policies", "import")]
public record GcloudDataprocAutoscalingPoliciesImportOptions(
[property: CliArgument] string AutoscalingPolicy,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--source")]
    public string? Source { get; set; }
}