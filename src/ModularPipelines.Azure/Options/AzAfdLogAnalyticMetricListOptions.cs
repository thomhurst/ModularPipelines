using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "log-analytic", "metric", "list")]
public record AzAfdLogAnalyticMetricListOptions(
[property: CommandSwitch("--custom-domains")] string CustomDomains,
[property: CommandSwitch("--date-time-begin")] string DateTimeBegin,
[property: CommandSwitch("--date-time-end")] string DateTimeEnd,
[property: CommandSwitch("--granularity")] string Granularity,
[property: CommandSwitch("--metrics")] string Metrics,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--protocols")] string Protocols,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--continents")]
    public string? Continents { get; set; }

    [CommandSwitch("--country-or-regions")]
    public int? CountryOrRegions { get; set; }

    [CommandSwitch("--group-by")]
    public string? GroupBy { get; set; }
}

