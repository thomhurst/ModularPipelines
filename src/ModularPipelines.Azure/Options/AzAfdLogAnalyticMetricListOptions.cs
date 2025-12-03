using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("afd", "log-analytic", "metric", "list")]
public record AzAfdLogAnalyticMetricListOptions(
[property: CliOption("--custom-domains")] string CustomDomains,
[property: CliOption("--date-time-begin")] string DateTimeBegin,
[property: CliOption("--date-time-end")] string DateTimeEnd,
[property: CliOption("--granularity")] string Granularity,
[property: CliOption("--metrics")] string Metrics,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--protocols")] string Protocols,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--continents")]
    public string? Continents { get; set; }

    [CliOption("--country-or-regions")]
    public int? CountryOrRegions { get; set; }

    [CliOption("--group-by")]
    public string? GroupBy { get; set; }
}