using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "data-collection", "rule", "syslog", "add")]
public record AzMonitorDataCollectionRuleSyslogAddOptions(
[property: CliOption("--facility-names")] string FacilityNames,
[property: CliOption("--name")] string Name,
[property: CliOption("--streams")] string Streams
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--log-levels")]
    public string? LogLevels { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rule-name")]
    public string? RuleName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}