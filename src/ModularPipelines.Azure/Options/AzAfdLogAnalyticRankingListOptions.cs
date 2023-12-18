using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "log-analytic", "ranking", "list")]
public record AzAfdLogAnalyticRankingListOptions(
[property: CommandSwitch("--date-time-begin")] string DateTimeBegin,
[property: CommandSwitch("--date-time-end")] string DateTimeEnd,
[property: CommandSwitch("--max-ranking")] string MaxRanking,
[property: CommandSwitch("--metrics")] string Metrics,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--rankings")] string Rankings,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--custom-domains")]
    public string? CustomDomains { get; set; }
}