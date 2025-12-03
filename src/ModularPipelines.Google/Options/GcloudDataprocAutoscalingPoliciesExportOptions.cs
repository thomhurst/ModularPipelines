using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "autoscaling-policies", "export")]
public record GcloudDataprocAutoscalingPoliciesExportOptions(
[property: CliArgument] string AutoscalingPolicy,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }
}