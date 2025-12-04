using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd", "waf-log-analytic", "ranking", "list")]
public record AzAfdWafLogAnalyticRankingListOptions(
[property: CliOption("--date-time-begin")] string DateTimeBegin,
[property: CliOption("--date-time-end")] string DateTimeEnd,
[property: CliOption("--max-ranking")] string MaxRanking,
[property: CliOption("--metrics")] string Metrics,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--rankings")] string Rankings,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--actions")]
    public string? Actions { get; set; }

    [CliOption("--rule-types")]
    public string? RuleTypes { get; set; }
}