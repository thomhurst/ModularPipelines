using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "data-collection", "rule", "syslog", "update")]
public record AzMonitorDataCollectionRuleSyslogUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--facility-names")]
    public string? FacilityNames { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--log-levels")]
    public string? LogLevels { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rule-name")]
    public string? RuleName { get; set; }

    [CliOption("--streams")]
    public string? Streams { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}