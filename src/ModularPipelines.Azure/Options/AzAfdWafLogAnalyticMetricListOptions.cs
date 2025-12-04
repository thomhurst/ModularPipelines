using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd", "waf-log-analytic", "metric", "list")]
public record AzAfdWafLogAnalyticMetricListOptions(
[property: CliOption("--date-time-begin")] string DateTimeBegin,
[property: CliOption("--date-time-end")] string DateTimeEnd,
[property: CliOption("--granularity")] string Granularity,
[property: CliOption("--metrics")] string Metrics,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--actions")]
    public string? Actions { get; set; }

    [CliOption("--group-by")]
    public string? GroupBy { get; set; }

    [CliOption("--rule-types")]
    public string? RuleTypes { get; set; }
}