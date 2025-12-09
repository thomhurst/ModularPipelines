using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "data-collection", "rule", "windows-event-log", "add")]
public record AzMonitorDataCollectionRuleWindowsEventLogAddOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--streams")] string Streams,
[property: CliOption("--x-path-queries")] string XPathQueries
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rule-name")]
    public string? RuleName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}