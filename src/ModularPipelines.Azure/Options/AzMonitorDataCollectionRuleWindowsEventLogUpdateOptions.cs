using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "data-collection", "rule", "windows-event-log", "update")]
public record AzMonitorDataCollectionRuleWindowsEventLogUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rule-name")]
    public string? RuleName { get; set; }

    [CliOption("--streams")]
    public string? Streams { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--x-path-queries")]
    public string? XPathQueries { get; set; }
}