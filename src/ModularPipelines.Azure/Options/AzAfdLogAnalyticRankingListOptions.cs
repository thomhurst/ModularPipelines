using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("afd", "log-analytic", "ranking", "list")]
public record AzAfdLogAnalyticRankingListOptions(
[property: CliOption("--date-time-begin")] string DateTimeBegin,
[property: CliOption("--date-time-end")] string DateTimeEnd,
[property: CliOption("--max-ranking")] string MaxRanking,
[property: CliOption("--metrics")] string Metrics,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--rankings")] string Rankings,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--custom-domains")]
    public string? CustomDomains { get; set; }
}