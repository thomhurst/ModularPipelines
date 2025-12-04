using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "autoscale", "rule", "copy")]
public record AzMonitorAutoscaleRuleCopyOptions(
[property: CliOption("--autoscale-name")] string AutoscaleName,
[property: CliOption("--dest-schedule")] string DestSchedule,
[property: CliOption("--index")] string Index,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--source-schedule")]
    public string? SourceSchedule { get; set; }
}