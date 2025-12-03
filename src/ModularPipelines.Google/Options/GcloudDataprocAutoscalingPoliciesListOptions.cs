using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "autoscaling-policies", "list")]
public record GcloudDataprocAutoscalingPoliciesListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}