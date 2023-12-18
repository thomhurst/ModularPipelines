using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "waf-log-analytic", "ranking", "list")]
public record AzAfdWafLogAnalyticRankingListOptions(
[property: CommandSwitch("--date-time-begin")] string DateTimeBegin,
[property: CommandSwitch("--date-time-end")] string DateTimeEnd,
[property: CommandSwitch("--max-ranking")] string MaxRanking,
[property: CommandSwitch("--metrics")] string Metrics,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--rankings")] string Rankings,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--actions")]
    public string? Actions { get; set; }

    [CommandSwitch("--rule-types")]
    public string? RuleTypes { get; set; }
}